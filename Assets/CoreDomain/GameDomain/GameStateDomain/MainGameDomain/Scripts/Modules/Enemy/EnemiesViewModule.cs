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
        private readonly Func<EnemyDataScriptableObject, EnemyView> _createEnemyFunction;
        private const string EnemyWaveParentName = "EnemiesWaveParent";
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new(0.2f, 0.9f);
        private readonly Vector2 _enemiesGroupStartPosition;
        private List<EnemyView> _enemyViews = new();
        
        public EnemiesViewModule(IDeviceScreenService deviceScreenService, Func<EnemyDataScriptableObject, EnemyView> createEnemyFunction)
        {
            _createEnemyFunction = createEnemyFunction;
            _enemiesGroupStartPosition = deviceScreenService.ScreenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + deviceScreenService.ScreenCenterPointInWorldSpace;
        }

        public async UniTask DoEnemiesWaveSequence(EnemiesWaveSequenceData enemiesWave)
        {
            List<UniTask> enemiesTasks = new List<UniTask>();

            GameObject enemiesWaveParent = new GameObject(EnemyWaveParentName);
            enemiesWaveParent.transform.position = _enemiesGroupStartPosition;

            var enemiesGrid = enemiesWave.EnemiesGrid;
            var enemyParent = enemiesWaveParent.transform;
            var enemiesColumns = enemiesGrid.GetLength(0);
            var enemiesRows = enemiesGrid.GetLength(1);
            var centerScreenX = 0;
            var widthOfGrid = (enemiesColumns - 1) * (enemiesWave.CellSize + enemiesWave.SpaceBetweenColumns);
            var startX = centerScreenX-widthOfGrid * 0.5f;
            var startY = _enemiesGroupStartPosition.y;

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    var cellX = startX + enemiesWave.CellSize * j + enemiesWave.SpaceBetweenColumns * j;
                    var cellY = startY - enemiesWave.CellSize * i - enemiesWave.SpaceBetweenRows * i;
                    var cellPosition = new Vector2(cellX, cellY);
                    var cellLocalToParentPosition = enemyParent.transform.InverseTransformPoint(cellPosition);
                    enemiesTasks.Add(DoEnemySequence(enemiesGrid[j, i], enemyParent, cellLocalToParentPosition));
                }
            }

            
            var enemyParentPosition = enemyParent.transform.position;
            var moveEnemyParentTask = enemyParent.DOMove(enemyParentPosition - Vector3.right * enemyParentPosition.x, 3).SetLoops(-1, LoopType.Yoyo);
            await enemiesTasks;
            moveEnemyParentTask.Kill();
            GameObject.Destroy(enemiesWaveParent.gameObject);
        }

        public void KillEnemy(string enemyId)
        {
            var enemyToKill = _enemyViews.Find(x => x.Id == enemyId);
            KillEnemy(enemyToKill);
        }

        private async UniTask DoEnemySequence(EnemySequenceData enemySequenceData, Transform enemyParent, Vector2 cellLocalToParentPosition)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsBeforeEnter), ignoreTimeScale: false);
            var enemyView = CreateEnemy(enemySequenceData);
            enemyView.gameObject.SetActive(true);
            var enemyPathsData = enemySequenceData.EnemyPathsData;
            enemyView.transform.SetParent(enemyParent, true);
            var enterPath = GameObject.Instantiate(enemyPathsData.EnterPath, enemyParent, true); // check if can not instatiate this, and just use its prefab
            var cellLocalToWorldPosition = enemyParent.TransformPoint(cellLocalToParentPosition);
            MovePathEndPointToCellPosition(cellLocalToWorldPosition, enterPath);

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

        private EnemyView CreateEnemy(EnemySequenceData enemySequenceData)
        {
            var enemyView = _createEnemyFunction(enemySequenceData.EnemyPathsData.Enemy);
            _enemyViews.Add(enemyView);

            return enemyView;
        }
        
        private void KillEnemy(EnemyView enemyView)
        {
            _enemyViews.Remove(enemyView); 
            enemyView.Despawn();
        }
    }
}