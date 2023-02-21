using System;
using System.Collections.Generic;
using System.Threading;
using CoreDomain.Consts;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesViewModule
    {
        private const string EnemyWaveParentName = "EnemiesWaveParent";
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new(0.2f, 0.9f);
        
        private readonly Func<EnemyDataScriptableObject, EnemyView> _createEnemyFunction;
        private readonly Vector2 _enemiesGroupStartPosition;
        private readonly List<EnemyView> _enemyViews = new();
        private Transform _enemiesWaveParentTransform;
        private EnemiesWaveParent _enemiesWaveParent;
        private CancellationTokenSource _waveCancellationToken = new ();

        public EnemiesViewModule(IDeviceScreenService deviceScreenService, Func<EnemyDataScriptableObject, EnemyView> createEnemyFunction)
        {
            _createEnemyFunction = createEnemyFunction;
            _enemiesGroupStartPosition = deviceScreenService.ScreenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + deviceScreenService.ScreenCenterPointInWorldSpace;
        }

        public async UniTask StartEnemiesWaveSequence(EnemiesWaveSequenceData enemiesWave)
        {
            if (_enemiesWaveParentTransform == null)
            {
                SetupWaveParent();
            }

            var enemiesGrid = enemiesWave.EnemiesGrid;
            var enemiesColumns = enemiesGrid.GetLength(0);
            var enemiesRows = enemiesGrid.GetLength(1);
            var gridTopRightCorner = CalculateGridTopRightCorner(enemiesWave);
            var enemiesTasks = new List<UniTask>();
            
            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    var cellX = gridTopRightCorner.x + enemiesWave.CellSize * j + enemiesWave.SpaceBetweenColumns * j;
                    var cellY = gridTopRightCorner.y - enemiesWave.CellSize * i - enemiesWave.SpaceBetweenRows * i;
                    var cellPosition = new Vector2(cellX, cellY);
                    var cellLocalToParentPosition = _enemiesWaveParentTransform.InverseTransformPoint(cellPosition);
                    enemiesTasks.Add(DoEnemyFullSequence(enemiesGrid[j, i], cellLocalToParentPosition).SuppressCancellationThrow());
                }
            }
            
            await UniTask.WhenAll(enemiesTasks).AttachExternalCancellation(_waveCancellationToken.Token);
        }

        public void StopEnemiesWaveSequence()
        {
            _waveCancellationToken.Cancel();
        }

        private Vector2 CalculateGridTopRightCorner(EnemiesWaveSequenceData enemiesWave)
        {
            var centerScreenX = 0;
            var gridWidth = (enemiesWave.EnemiesGrid.GetLength(0) - 1) * (enemiesWave.CellSize + enemiesWave.SpaceBetweenColumns);
            var gridTopRightCorner = new Vector2(centerScreenX - gridWidth * 0.5f, _enemiesGroupStartPosition.y);

            return gridTopRightCorner;
        }

        private void SetupWaveParent()
        {
            _enemiesWaveParentTransform = new GameObject(EnemyWaveParentName).transform;
            _enemiesWaveParent = _enemiesWaveParentTransform.AddComponent<EnemiesWaveParent>();
            _enemiesWaveParentTransform.position = _enemiesGroupStartPosition;
            _enemiesWaveParent.StartHorizontalYoyoMoving();
        }

        public void KillEnemy(string enemyId)
        {
            var enemyToKill = _enemyViews.Find(x => x.Id == enemyId);
            KillEnemy(enemyToKill);
        }

        private async UniTask DoEnemyFullSequence(EnemySequenceData enemySequenceData, Vector2 cellLocalToParentPosition)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsBeforeEnter), ignoreTimeScale: false);
            
            var enemyView = CreateEnemy(enemySequenceData.EnemyPathsData.Enemy);

            await DoEnemyEnterPathSequence(enemySequenceData.EnemyPathsData.EnterPath, cellLocalToParentPosition, enemyView);
            await UniTask.Delay(TimeSpan.FromSeconds(enemySequenceData.SecondsInIdle), ignoreTimeScale: false);
            await DoEnemyExitPathSequence(enemySequenceData.EnemyPathsData.ExitPath, cellLocalToParentPosition, enemyView);
            
            enemyView.gameObject.SetActive(false);
        }

        private async UniTask DoEnemyEnterPathSequence(PathCreator enterPath, Vector2 cellLocalToParentPosition, EnemyView enemyView)
        {
            var pathLastPoint = enterPath.path.GetPoint(enterPath.path.NumPoints - 1);
            await enemyView.FollowPath(enterPath.path, () => GetDeltaFromPathToCellWorldPosition(pathLastPoint, cellLocalToParentPosition));
            await enemyView.RotateTowardsDirection(ConstsHandler.Vector3Up);
        }
        
        private async UniTask DoEnemyExitPathSequence(PathCreator exitPath, Vector2 cellLocalToParentPosition, EnemyView enemyView)
        {
            await enemyView.RotateTowardsDirection(exitPath.path.GetDirection(0));
            var pathStartPoint = exitPath.path.GetPoint(0);
            await enemyView.FollowPath(exitPath.path, () => GetDeltaFromPathToCellWorldPosition(pathStartPoint, cellLocalToParentPosition));
        }

        private Vector3 GetDeltaFromPathToCellWorldPosition(Vector3 pointOnPathSnappedToCell, Vector3 cellLocalToParentPosition)
        {
            var cellLocalToWorldPosition = _enemiesWaveParentTransform.TransformPoint(cellLocalToParentPosition);
            var pathDeltaFromCellPosition = cellLocalToWorldPosition - pointOnPathSnappedToCell;
            return pathDeltaFromCellPosition;
        }

        private EnemyView CreateEnemy(EnemyDataScriptableObject enemyScriptableObject)
        {
            var enemyView = _createEnemyFunction(enemyScriptableObject);
            enemyView.gameObject.SetActive(true);
            enemyView.transform.SetParent(_enemiesWaveParentTransform, true);
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