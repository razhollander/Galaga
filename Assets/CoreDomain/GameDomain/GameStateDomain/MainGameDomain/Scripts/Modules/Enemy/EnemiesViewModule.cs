using System;
using System.Collections;
using System.Collections.Generic;
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
        public EnemiesViewModule(IDeviceScreenService deviceScreenService)
        {
            _enemiesGroupStartPosition = deviceScreenService.ScreenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + deviceScreenService.ScreenCenterPointInWorldSpace;
        }

        private async UniTask DoEnemiesWaveSequence(EnemiesWaveSequenceData enemiesWave)
        {
            List<UniTask> enemiesTasks = new List<UniTask>();

            GameObject enemiesWaveParent = new GameObject(EnemyWaveParentName);
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
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }

        private void MovePathStartPointToCellPosition(Vector2 cellPosition, PathCreator pathCreator)
        {
            var pathDeltaFromCellPosition = cellPosition - pathCreator.path.GetPoint(0).ToVector2XY();
            pathCreator.transform.position += (Vector3) pathDeltaFromCellPosition;
        }
    }
}