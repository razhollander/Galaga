using System;
using Client;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.MainGameScreen.Enemy
{
    public class EnemiesController : IDisposable
    {
        #region --- Constants ---

        private const string ENEMY_PARENT_GO_NAME = "EnemiesParent";

        #endregion


        #region --- Members ---
        
        private readonly EnemiesModel _model;

        private EnemiesView _enemiesView;
        private readonly IClient _client;
        private Transform _enemiesParentTransform;

        #endregion


        #region --- Construction ---

        public EnemiesController(IClient client)
        {
            _client = client;
            _model = new EnemiesModel(_client);
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            _model?.Dispose();
            DestroyAssets();
        }

        public void Setup()
        {
            CreateEnemiesParent();
        }

        #endregion


        #region --- Private Methods ---

        private void CreateEnemiesParent()
        {
            var enemyGO = _client.AssetBundleSystem.LoadAssetFromBundle<GameObject>(_model.ENEMY_BUNDLE_PATH, _model.ENEMY_ASSET_NAME);

            var enemyView = enemyGO.GetComponent<EnemyView>();

            _enemiesParentTransform = new GameObject(ENEMY_PARENT_GO_NAME).transform;
            _model.EnemiesParentTransform = _enemiesParentTransform;
            _enemiesView = _enemiesParentTransform.gameObject.AddComponent<EnemiesView>();
            _enemiesView.Setup(_client, _model, OnEnemyHitByBullet, enemyView);
        }

        private void DestroyAssets()
        {
            Object.Destroy(_enemiesView.gameObject);
        }

        #endregion


        #region --- Event Handler ---

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

        #endregion
    }
}