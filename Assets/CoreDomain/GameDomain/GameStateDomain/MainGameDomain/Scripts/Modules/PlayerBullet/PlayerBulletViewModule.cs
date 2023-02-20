using System;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletViewModule
    {
        private readonly Action<PlayerBulletView> _onDestroyBullet;
        private List<PlayerBulletView> _bulletViews = new ();

        public PlayerBulletViewModule(Action<PlayerBulletView> onDestroyBullet)
        {
            _onDestroyBullet = onDestroyBullet;
        }

        public void FireBullet(PlayerBulletView bulletView, Vector3 bulletStartPosition)
        {
            _bulletViews.Add(bulletView);
            bulletView.gameObject.SetActive(true);
            bulletView.StartMoving(bulletStartPosition);
        }

        public void DestroyBullet(string bulletId)
        {
            var bulletView = _bulletViews.Find(x => x.Id == bulletId);
            _bulletViews.Remove(bulletView);
            bulletView.gameObject.SetActive(false);
            _onDestroyBullet(bulletView);
        }
    }
}
