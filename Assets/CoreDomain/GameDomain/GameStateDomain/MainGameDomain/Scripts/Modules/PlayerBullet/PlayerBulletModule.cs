using System;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletModule : IPlayerBulletModule
    {
        private readonly PlayerBulletHitCommand.Factory _playerBulletHitCommandFactory;
        private readonly IAudioService _audioService;
        private PlayerBulletViewModule _playerBulletViewModule;
        private PlayerBulletCreator _playerBulletCreator;
        private Dictionary<string, PlayerBulletData> _playerBulletsData = new ();
        
        public PlayerBulletModule(PlayerBulletHitCommand.Factory playerBulletHitCommandFactory, PlayerBulletPool.Factory playerBulletPoolFactory, IAudioService audioService)
        {
            _playerBulletHitCommandFactory = playerBulletHitCommandFactory;
            _audioService = audioService;
            _playerBulletCreator = new PlayerBulletCreator(playerBulletPoolFactory);
            _playerBulletViewModule = new PlayerBulletViewModule();
        }

        public void FireBullet(Vector3 startPosition)
        {
            var bulletView = CreateBullet();
            _playerBulletViewModule.FireBullet(bulletView, startPosition);
            _audioService.PlayAudio(AudioClipName.FireSoundFXName, AudioChannelType.Fx, AudioPlayType.OneShot);
        }

        private PlayerBulletView CreateBullet()
        {
            var bulletId = Guid.NewGuid().ToString();
            _playerBulletsData.Add(bulletId, new PlayerBulletData(bulletId));
            var bulletView = _playerBulletCreator.CreateBullet();
            bulletView.Setup(bulletId, OnBulletHit, OnBulletOutOfScreen);

            return bulletView;
        }

        public void DestroyBullet(string bulletId)
        {
             _playerBulletsData.Remove(bulletId);
            _playerBulletViewModule.DestroyBullet(bulletId);
        }

        public bool IsBulletExist(string bulletId)
        {
            return _playerBulletsData.ContainsKey(bulletId);
        }

        public void Dispose()
        {
            DestroyAllBullets();
        }

        private void DestroyAllBullets()
        {
            _playerBulletsData.ForEach(x => _playerBulletViewModule.DestroyBullet(x.Key));
            _playerBulletsData.Clear();
        }

        private void OnBulletHit(PlayerBulletView playerBulletView, Collider2D hitWithCollider2D)
        {
            _playerBulletHitCommandFactory.Create(new PlayerBulletHitCommandData(hitWithCollider2D, playerBulletView)).Execute();
        }
        
        private void OnBulletOutOfScreen(PlayerBulletView playerBulletView)
        {
            DestroyBullet(playerBulletView.Id);
        }
    }
}