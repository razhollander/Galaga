using System;
using Client;
using Features.MainGameScreen.GameLogicManager;
using Handlers;
using CoreDomain;
using UnityEngine;

namespace Features.MainGameScreen.Enemy
{
    public class EnemiesModel : IDisposable, ISavableObject
    {
        #region --- Constants ---

        private const float START_POSITION_OF_SCREEN_RATIO = 5f / 8;
        private const string ENEMIES_SAVE_KAY = "EnemiesSave";

        #endregion


        #region --- Members ---

        public readonly float EnemiesHorizontalSpeed = 2f;
        public readonly float EnemiesVerticalSpeed = 2f;
        public readonly int ENEMIES_COLUMNS = 5;
        public readonly int ENEMIES_ROWS = 3;

        public readonly int EnemiesStepSleepTimeInSeconds = 1;
        public readonly string ENEMY_ASSET_NAME = "EnemySpaceship";

        public readonly string ENEMY_BUNDLE_PATH = "MainGameState/Enemy";
        public Transform EnemiesParentTransform;

        public Vector2 EnemiesParentStartPosition;
        public int MoveDir = 1; // 1 = right, -1 = left
        
        private bool[,] _enemiesAlive; // true = alive, false = dead
        private readonly IClient _client;
        private int _numOfEnemiesAlive;

        #endregion


        #region --- Properties ---

        public bool AreEnemiesEnabled { get; private set; }

        #endregion


        #region --- Construction ---

        public EnemiesModel(IClient client)
        {
            _client = client;

            EnemiesParentStartPosition =
                new Vector2(0, _client.CameraManager.ScreenBounds.y * START_POSITION_OF_SCREEN_RATIO);

            _enemiesAlive = new bool[ENEMIES_ROWS, ENEMIES_COLUMNS];
            _numOfEnemiesAlive = ENEMIES_ROWS * ENEMIES_COLUMNS;
            AreEnemiesEnabled = true;

            for (var i = 0; i < ENEMIES_ROWS; i++)
            {
                for (var k = 0; k < ENEMIES_COLUMNS; k++)
                {
                    _enemiesAlive[i, k] = true;
                }
            }

            AddListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            RemoveListeners();
        }

        public bool IsEnemyAlive(int enemyRow, int enemyColumn)
        {
            return _enemiesAlive[enemyRow, enemyColumn];
        }

        public void Load()
        {
            var enemiesData = SaveLocallyHandler.LoadObject<EnemiesData>(ENEMIES_SAVE_KAY);
            _enemiesAlive = enemiesData.EnemiesAlive;

            MoveDir = enemiesData.MoveDir;
            EnemiesParentStartPosition = new Vector2(enemiesData.EnemiesParentPosition[0], enemiesData.EnemiesParentPosition[1]);

            _numOfEnemiesAlive = 0;

            for (var i = 0; i < ENEMIES_ROWS; i++)
            {
                for (var k = 0; k < ENEMIES_COLUMNS; k++)
                {
                    if (_enemiesAlive[i, k])
                    {
                        _numOfEnemiesAlive++;
                    }
                }
            }
        }

        public void Save()
        {
            var parentPosition = EnemiesParentTransform.position;

            SaveLocallyHandler.SaveObject(ENEMIES_SAVE_KAY, new EnemiesData(
                new[] {parentPosition.x, parentPosition.y}, _enemiesAlive, MoveDir));
        }

        public void SetEnemyDead(int enemyRow, int enemyColumn)
        {
            _enemiesAlive[enemyRow, enemyColumn] = false;
            _numOfEnemiesAlive--;

            CheckForPlayerWin();
        }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _client.GameSaverService.RegisterSavedObject(this);
            _client.Broadcaster.Add<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Add<PlayerWinEvent>(OnPlayerWin);
        }

        private void CheckForPlayerWin()
        {
            if (_numOfEnemiesAlive == 0)
            {
                _client.Broadcaster.Broadcast(new PlayerWinEvent());
            }
        }

        private void RemoveListeners()
        {
            _client.GameSaverService.UnregisterSavedObject(this);
            _client.Broadcaster.Add<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Add<PlayerWinEvent>(OnPlayerWin);
        }

        private void SetEnemiesDisabled()
        {
            AreEnemiesEnabled = false;
        }

        #endregion


        #region --- Event Handler ---

        private void OnPlayerLose(PlayerLoseEvent obj)
        {
            SetEnemiesDisabled();
        }

        private void OnPlayerWin(PlayerWinEvent obj)
        {
            SetEnemiesDisabled();
        }

        #endregion


        #region --- Inner Classes ---

        private class EnemiesData
        {
            #region --- Members ---

            public readonly int MoveDir;
            public readonly bool[,] EnemiesAlive;
            public readonly float[] EnemiesParentPosition; // acts like Vector 2

            #endregion


            #region --- Construction ---

            public EnemiesData(float[] enemiesParentPosition, bool[,] enemiesAlive, int moveDir)
            {
                MoveDir = moveDir;
                EnemiesParentPosition = enemiesParentPosition;
                EnemiesAlive = enemiesAlive;
            }

            #endregion
        }

        #endregion
    }
}