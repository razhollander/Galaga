using System;
using Client;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.MainGameScreen.Bullet
{
    public class BulletController : IDisposable
    {
        #region --- Members ---

        private readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        private bool _hasAvailableBullet = true;
        private readonly BulletModel _model;
        private BulletView _bulletView;
        private GameObject _bulletGO;

        private readonly IClient _client;

        #endregion


        #region --- Construction ---

        public BulletController(IClient client)
        {
            _client = client;
            _model = new BulletModel(_client);
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            _model.Dispose();
            DestroyAssets();
        }

        public void Setup(BulletView bulletView)
        {
            _bulletView = bulletView;
            _bulletGO = _bulletView.gameObject;
            _bulletGO.SetActive(false);
            _bulletView.Setup(_client, _model, OnHitEnemy, OnBulletOutOfScreen, _model.BulletSpeed);
        }

        public void ShootBullet(Vector3 startPos)
        {
            if (!_model.IsBulletEnabled || !_hasAvailableBullet)
            {
                return;
            }

            _hasAvailableBullet = false;
            _bulletGO.transform.position = startPos;
            _bulletGO.SetActive(true);
            _bulletView.StartMoving();
        }

        #endregion


        #region --- Private Methods ---

        private void DestroyAssets()
        {
            Object.Destroy(_bulletGO);
        }

        private void DisableBullet()
        {
            _bulletView.StopMoving();
            _bulletGO.SetActive(false);
            _hasAvailableBullet = true;
        }

        #endregion


        #region --- Event Handler ---

        private void OnBulletOutOfScreen()
        {
            DisableBullet();
        }

        private void OnHitEnemy(Collider2D other)
        {
            if (!_model.IsBulletEnabled || other.gameObject.layer != ENEMY_LAYER)
            {
                return;
            }

            other.GetComponent<EnemyView>().HitByBullet();
            DisableBullet();
        }

        #endregion
    }
}