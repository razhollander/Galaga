using System;
using System.Collections;
using Client;
using Unity.Mathematics;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesView : MonoBehaviour
    {
        private const float HORIZONTAL_SPACE_BETWEEN_ENEMIES = 0.2f;
        private const float VERTICAL_SPACE_BETWEEN_ENEMIES = 0.1f;
        
        private Action<EnemyView> _onEnemyHit;
        private Coroutine _moveCoroutine;
        private EnemiesModel _model;
        private EnemyView[,] _enemyViewArray;
        private float _enemySizeKeptFromBounds;
        private IClient _client;
        private Transform _mostLeftEnemy;

        private Transform _mostRightEnemy;
        private Transform _transform;
        private Vector3 _enemySize;
        
        
        private void Awake()
        {
            _transform = transform;
        }

        private void OnDestroy()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
        }
        
        public void Setup(IClient client, EnemiesModel model, Action<EnemyView> onEnemyHit, EnemyView enemyView)
        {
            // _client = client;
            // _model = model;
            // _onEnemyHit = onEnemyHit;
            // _enemySize = enemyView.EnemySpriteRenderer.bounds.size;
            // _enemySizeKeptFromBounds = _enemySize.x / 2;
            // _enemyViewArray = new EnemyView[_model.ENEMIES_ROWS, _model.ENEMIES_COLUMNS];
            //
            // CreateEnemies(enemyView.gameObject);
            // UpdateMostSidedEnemies();
            //
            // _moveCoroutine = StartCoroutine(Move());
        }
        
        public void UpdateMostSidedEnemies()
        {
            if (!_model.AreEnemiesEnabled)
            {
                return;
            }

            var mostRightIndex = new int2();
            var mostLeftIndex = new int2();
            var didFoundLeftEnemy = false;

            // we go from left to right and save the first enemy alive as the the most left enemy, and we continue to look for the most right one
            for (var k = 0; k < _model.ENEMIES_COLUMNS; k++)
            {
                for (var i = 0; i < _model.ENEMIES_ROWS; i++)
                {
                    if (!_model.IsEnemyAlive(i, k))
                    {
                        continue;
                    }

                    mostRightIndex = new int2(i, k);

                    if (!didFoundLeftEnemy)
                    {
                        mostLeftIndex = mostRightIndex;
                        didFoundLeftEnemy = true;
                    }
                }
            }

            _mostRightEnemy = _enemyViewArray[mostRightIndex.x, mostRightIndex.y].transform;
            _mostLeftEnemy = _enemyViewArray[mostLeftIndex.x, mostLeftIndex.y].transform;
        }

        private void CreateEnemies(GameObject enemyGO)
        {
            var numOfEnemyRows = _model.ENEMIES_ROWS;
            var numOfEnemyColumns = _model.ENEMIES_COLUMNS;

            var startX = -(numOfEnemyColumns - 1) * (_enemySize.x + HORIZONTAL_SPACE_BETWEEN_ENEMIES) /
                         2; // so we create enemies from horizontal center

            var startY = -_enemySize.y / 2;

            for (var i = 0; i < numOfEnemyRows; i++)
            {
                for (var k = 0; k < numOfEnemyColumns; k++)
                {
                    if (!_model.IsEnemyAlive(i, k))
                    {
                        continue;
                    }

                    var currEnemy = Instantiate(enemyGO, transform, true);
                    var currEnemyView = currEnemy.GetComponent<EnemyView>();
                    currEnemyView.Setup(i, k, OnEnemyHit);
                    _enemyViewArray[i, k] = currEnemyView;

                    currEnemy.transform.position =
                        new Vector3(startX + _enemySize.x * k + HORIZONTAL_SPACE_BETWEEN_ENEMIES * k,
                            startY - _enemySize.y * i - VERTICAL_SPACE_BETWEEN_ENEMIES * i, 0);
                }
            }

            _transform.position = _model.EnemiesParentStartPosition;
        }

        private IEnumerator Move()
        {
            var waitForSeconds = new WaitForSeconds(_model.EnemiesStepSleepTimeInSeconds);

            while (true)
            {
                if (_model.AreEnemiesEnabled)
                {
                    var enemy = _model.MoveDir > 0 ? _mostRightEnemy : _mostLeftEnemy;

                    if (enemy == null) // no more enemies not sure if needed
                    {
                        yield break;
                    }

                    var moveDelta = _model.MoveDir * _model.EnemiesHorizontalSpeed;
                    var enemyNewXPos = enemy.position.x + moveDelta;

                    if (_client.CameraManager.IsInScreenHorizontalBounds(enemyNewXPos, _enemySizeKeptFromBounds))
                    {
                        _transform.Translate(moveDelta, 0, 0);
                    }
                    else
                    {
                        _transform.Translate(0, -_model.EnemiesVerticalSpeed, 0);
                        _model.MoveDir *= -1;
                    }
                }

                yield return waitForSeconds;
            }
        }
        
        private void OnEnemyHit(EnemyView enemyView)
        {
            _onEnemyHit?.Invoke(enemyView);
        }
    }
}