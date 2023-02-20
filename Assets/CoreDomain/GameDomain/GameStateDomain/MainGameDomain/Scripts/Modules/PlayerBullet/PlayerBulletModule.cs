using System;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletModule : IPlayerBulletModule
    {
        private readonly PlayerBulletHitCommand.Factory _playerBulletHitCommandFactory;
        private PlayerBulletViewModule _playerBulletViewModule;
        private PlayerBulletCreator _playerBulletCreator;
        private Dictionary<string, PlayerBulletData> _playerBulletsData = new ();
        
        public PlayerBulletModule(PlayerBulletHitCommand.Factory playerBulletHitCommandFactory, PlayerBulletPool.Factory playerBulletPoolFactory)
        {
            _playerBulletHitCommandFactory = playerBulletHitCommandFactory;
            _playerBulletCreator = new PlayerBulletCreator(playerBulletPoolFactory);
            _playerBulletViewModule = new PlayerBulletViewModule(_playerBulletCreator.DestroyBullet);
        }

        public void FireBullet(Vector3 startPosition)
        {
            var bulletId = Guid.NewGuid().ToString();

            _playerBulletsData.Add(bulletId, new PlayerBulletData(bulletId));
            var bulletView = _playerBulletCreator.CreateBullet();
            bulletView.Setup(bulletId, OnBulletHit, OnBulletOutOfScreen);
            _playerBulletViewModule.FireBullet(bulletView, startPosition);
        }

        public void DestroyBullet(string bulletId)
        {
            if (!_playerBulletsData.ContainsKey(bulletId))
            {
                return;
            }

            _playerBulletsData.Remove(bulletId);
            _playerBulletViewModule.DestroyBullet(bulletId);
        }

        private void OnBulletHit(PlayerBulletView playerBulletView, Collider2D hitWithCollider2D)
        {
            _playerBulletHitCommandFactory.Create(new PlayerBulletHitCommandData(hitWithCollider2D, playerBulletView)).Execute();
        }
        
        private void OnBulletOutOfScreen(PlayerBulletView playerBulletView)
        {
            _playerBulletsData.Remove(playerBulletView.Id);
            _playerBulletViewModule.DestroyBullet(playerBulletView.Id);
        }
    }
}