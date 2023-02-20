using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using PathCreation;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesViewModule
    {
        private const string EnemyWaveParentName = "EnemiesWaveParent";
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new(0.2f, 0.9f);
        private readonly Vector2 _enemiesGroupStartPosition;
        private List<EnemyView> _enemyViews = new();
        
        public EnemiesViewModule(IDeviceScreenService deviceScreenService)
        {
            _enemiesGroupStartPosition = deviceScreenService.ScreenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + deviceScreenService.ScreenCenterPointInWorldSpace;
        }

        public async UniTask DoEnemiesWaveSequence(EnemyView[,] enemyViews, EnemiesWaveSequenceData enemiesWave)
        {
            _enemyViews.AddRange(enemyViews.Cast<EnemyView>().ToList());
            
            List<UniTask> enemiesTasks = new List<UniTask>();

            GameObject enemiesWaveParent = new GameObject(EnemyWaveParentName);
            enemiesWaveParent.transform.position = _enemiesGroupStartPosition;

            var enemiesGrid = enemiesWave.EnemiesGrid;
            var enemyParent = enemiesWaveParent.transform;
            var enemiesRows = enemiesGrid.GetLength(0);
            var enemiesColumns = enemiesGrid.GetLength(1);

            var startX = -(enemiesColumns - 1) * (enemiesWave.CellSize + enemiesWave.SpaceBetweenColumns) * 0.5f; // so we create enemies from horizontal center
            var startY = _enemiesGroupStartPosition.y;

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    var cellX = startX + enemiesWave.CellSize * j + enemiesWave.SpaceBetweenColumns * j;
                    var cellY = startY - enemiesWave.CellSize * i - enemiesWave.SpaceBetweenRows * i;
                    var cellPosition = new Vector2(cellX, cellY);

                    enemiesTasks.Add(DoEnemySequence(enemyViews[i,j], enemiesGrid[i, j], enemyParent, cellPosition));
                }
            }

            var enemyParentPosition = enemyParent.transform.position;
            var moveEnemyParentTask = enemyParent.DOMove(enemyParentPosition - Vector3.right * enemyParentPosition.x, 3).SetLoops(-1, LoopType.Yoyo);
            await enemiesTasks;
            moveEnemyParentTask.Kill();
            KillAllEnemies();
            GameObject.Destroy(enemiesWaveParent.gameObject);
        }

        private void KillAllEnemies()
        {
            _enemyViews.ForEach(KillEnemy);
        }

        private async UniTask DoEnemySequence(EnemyView enemyView, EnemySequenceData enemySequenceData, Transform enemyParent, Vector2 cellPosition)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsBeforeEnter), ignoreTimeScale: false);
            enemyView.gameObject.SetActive(true);
            var enemyPathsData = enemySequenceData.EnemyPathsData;
            enemyView.transform.SetParent(enemyParent, true);
            var enterPath = GameObject.Instantiate(enemyPathsData.EnterPath, enemyParent, true); // check if can not instatiate this, and just use its prefab
            MovePathEndPointToCellPosition(cellPosition, enterPath);

            await enemyView.FollowPath(enterPath.path);
            await enemyView.RotateTowardsDirection(Vector3.up);

            GameObject.Destroy(enterPath.gameObject);
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsInIdle), ignoreTimeScale: false);
            var exitPath = GameObject.Instantiate(enemyPathsData.ExitPath, enemyParent, true);

            await enemyView.RotateTowardsDirection(exitPath.path.GetDirection(0));
            MovePathStartPointToCellPosition(enemyView.transform.position, exitPath);
            await enemyView.FollowPath(exitPath.path);
            GameObject.Destroy(exitPath.gameObject);
            KillEnemy(enemyView.Id);
        }

        private void MovePathEndPointToCellPosition(Vector2 cellPosition, PathCreator pathCreator)
        {
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }

        private void MovePathStartPointToCellPosition(Vector2 cellPosition, PathCreator pathCreator)
        {
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(0).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }

        public void KillEnemy(string enemyId)
        {
            var enemyToKill = _enemyViews.Find(x => x.Id == enemyId);
            KillEnemy(enemyToKill);
        }

        private void KillEnemy(EnemyView enemyView)
        {
            _enemyViews.Remove(enemyView); 
            enemyView.Despawn();
        }
    }
}