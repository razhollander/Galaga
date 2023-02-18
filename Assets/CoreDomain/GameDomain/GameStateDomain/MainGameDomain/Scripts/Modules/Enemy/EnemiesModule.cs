using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
using PathCreation;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesModule : IEnemiesModule
    {
        private const string ENEMY_PARENT_GO_NAME = "EnemiesParent";
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new (0.2f, 0.9f);

        private readonly EnemiesModel _model;

        private EnemiesView _enemiesView;
        private Transform _enemiesParentTransform;
        private readonly Vector2 _enemiesGroupStartPosition;

        public EnemiesModule(IDeviceScreenService deviceScreenService)
        {
            _enemiesGroupStartPosition = deviceScreenService.ScreenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + deviceScreenService.ScreenCenterPointInWorldSpace;
        }

        public void Dispose()
        {
            _model?.Dispose();
            DestroyAssets();
        }

        // private void CreateEnemiesParent()
        // {
        //     var enemyGO = _client.AssetBundleLoaderService.LoadGameObjectAssetFromBundle(_model.ENEMY_BUNDLE_PATH, _model.ENEMY_ASSET_NAME);
        //
        //     var enemyView = enemyGO.GetComponent<EnemyView>();
        //
        //     _enemiesParentTransform = new GameObject(ENEMY_PARENT_GO_NAME).transform;
        //     _model.EnemiesParentTransform = _enemiesParentTransform;
        //     _enemiesView = _enemiesParentTransform.gameObject.AddComponent<EnemiesView>();
        //     _enemiesView.Setup(_client, _model, OnEnemyHitByBullet, enemyView);
        // }

        private void DestroyAssets()
        {
            Object.Destroy(_enemiesView.gameObject);
        }

        private void OnEnemyHitByBullet(EnemyView enemyView)
        {
            if (!_model.AreEnemiesEnabled)
            {
                return;
            }
            
            _model.SetEnemyDead(enemyView.RowIndex, enemyView.ColumnIndex);
            _enemiesView.UpdateMostSidedEnemies();
            Object.Destroy(enemyView.gameObject);
        }

        public async UniTaskVoid DoEnemiesWavesSequence(EnemiesWaveSequenceData[] enemiesWaveSequenceData)
        {
            foreach (var enemiesWave in enemiesWaveSequenceData)
            {
                await DoEnemiesWaveSequence(enemiesWave);
            }
        }

        private async UniTask DoEnemiesWaveSequence(EnemiesWaveSequenceData enemiesWave)
        {
            List<UniTask> enemiesTasks = new List<UniTask>();
            
            GameObject enemiesWaveParent = new GameObject("EnemiesWaveParent");
            enemiesWaveParent.transform.position = _enemiesGroupStartPosition;
            
            var enemiesGrid = enemiesWave.EnemiesGrid;
            var enemyParent = enemiesWaveParent.transform;
            var numberOfRows = enemiesGrid.GetLength(0);
            var numberOfColumns = enemiesGrid.GetLength(1);
        
            var startX = -(numberOfColumns - 1) * (enemiesWave.CellSize + enemiesWave.SpaceBetweenColumns) * 0.5f; // so we create enemies from horizontal center
            var startY = _enemiesGroupStartPosition.y;

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                   var cellPosition =
                        new Vector2(startX + enemiesWave.CellSize * j + enemiesWave.SpaceBetweenColumns * j,
                            startY - enemiesWave.CellSize * i - enemiesWave.SpaceBetweenRows * i);
                 
                    enemiesTasks.Add(DoEnemySequence(enemiesGrid[i, j], enemyParent, cellPosition));
                }
            }

            var enemyParentPosition = enemyParent.transform.position;
            var moveEnemyParentTask = enemyParent.DOMove(enemyParentPosition - Vector3.right * enemyParentPosition.x, 3).SetLoops(-1, LoopType.Yoyo);
            await enemiesTasks;
            moveEnemyParentTask.Kill();
            GameObject.Destroy(enemiesWaveParent);
        }
        
        // private void CreateEnemies(GameObject enemyGO)
        // {
        //     var numOfEnemyRows = _model.ENEMIES_ROWS;
        //     var numOfEnemyColumns = _model.ENEMIES_COLUMNS;
        //
        //     var startX = -(numOfEnemyColumns - 1) * (_enemySize.x + HORIZONTAL_SPACE_BETWEEN_ENEMIES) /
        //                  2; // so we create enemies from horizontal center
        //
        //     var startY = -_enemySize.y / 2;
        //
        //     for (var i = 0; i < numOfEnemyRows; i++)
        //     {
        //         for (var k = 0; k < numOfEnemyColumns; k++)
        //         {
        //             var currEnemy = Instantiate(enemyGO, transform, true);
        //             var currEnemyView = currEnemy.GetComponent<EnemyView>();
        //             currEnemyView.Setup(i, k, OnEnemyHit);
        //             _enemyViewArray[i, k] = currEnemyView;
        //
        //             currEnemy.transform.position =
        //                 new Vector3(startX + _enemySize.x * k + HORIZONTAL_SPACE_BETWEEN_ENEMIES * k,
        //                     startY - _enemySize.y * i - VERTICAL_SPACE_BETWEEN_ENEMIES * i, 0);
        //         }
        //     }
        //
        //     _transform.position = _model.EnemiesParentStartPosition;
        // }

        private async UniTask DoEnemySequence(EnemySequenceData enemySequenceData, Transform enemyParent, Vector2 cellPosition)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsBeforeEnter), ignoreTimeScale: false);

            var enemyPathsData = enemySequenceData.EnemyPathsData;
            var enemyView = GameObject.Instantiate(enemyPathsData.Enemy, enemyParent, true);
            var enterPath = GameObject.Instantiate(enemyPathsData.EnterPath, enemyParent, true);
            var pos = cellPosition;

            MovePathEndPointToCellPosition(pos, enterPath);

            await enemyView.FollowPath(enterPath.path);
            await enemyView.RotateTowardsDirection(Vector3.up);
            
            GameObject.Destroy(enterPath);
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsInIdle), ignoreTimeScale: false);
            var exitPath = GameObject.Instantiate(enemyPathsData.ExitPath, enemyParent, true);
            MovePathStartPointToCellPosition(pos, exitPath);
            await enemyView.RotateTowardsDirection(exitPath.path.GetDirection(0));
            await enemyView.FollowPath(exitPath.path);
        }

        private void MovePathEndPointToCellPosition(Vector2 cellPosition, PathCreator pathCreator)
        {
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(pathCreator.path.NumPoints-1).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }
        
        private void MovePathStartPointToCellPosition(Vector2 cellPosition, PathCreator pathCreator)
        {
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(0).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }
    }
}