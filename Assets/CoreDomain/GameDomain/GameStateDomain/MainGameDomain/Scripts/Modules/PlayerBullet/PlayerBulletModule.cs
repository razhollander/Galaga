using System.Collections;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletModule : IPlayerBulletModule
    {
        private readonly IAssetBundleLoaderService _assetBundleLoaderService;
        private readonly PlayerBulletHitCommand.Factory _playerBulletHitCommandFactory;
        private PlayerBulletViewModule _playerBulletViewModule;
        private PlayerBulletCreator _playerBulletCreator;
        
        public PlayerBulletModule(IAssetBundleLoaderService assetBundleLoaderService, PlayerBulletHitCommand.Factory playerBulletHitCommandFactory)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
            _playerBulletHitCommandFactory = playerBulletHitCommandFactory;
            _playerBulletViewModule = new PlayerBulletViewModule();
            _playerBulletCreator = new PlayerBulletCreator(_assetBundleLoaderService);
        }

        public void FireBullet(Vector3 startPosition)
        {
            var bulletView = _playerBulletCreator.CreateBullet();
            _playerBulletViewModule.FireBullet(bulletView, startPosition);
        }

        public void BulletHit(PlayerBulletView playerBulletView)
        {
            _playerBulletViewModule.BulletHit(playerBulletView);
        }

        private void OnBulletHit(PlayerBulletView playerBulletView, Collider2D hitWithCollider2D)
        {
            _playerBulletHitCommandFactory.Create(new PlayerBulletHitCommandData(hitWithCollider2D, playerBulletView)).Execute().Forget();
        }
    }
}

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public interface IPlayerBulletModule
    {
        void FireBullet(Vector3 startPosition);
        void BulletHit(PlayerBulletView playerBulletView);
    }
}
