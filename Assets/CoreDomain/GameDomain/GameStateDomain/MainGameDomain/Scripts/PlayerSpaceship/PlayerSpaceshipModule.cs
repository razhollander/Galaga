using CoreDomain.Services;
using Features.MainGameScreen.Bullet;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipModule : IPlayerSpaceshipModule
    {
        private const string BULLET_ASSET_NAME = "PlayerBullet";
        private const string PLAYER_ASSET_NAME = "PlayerSpaceship";
        private const string PLAYER_BUNDLE_PATH = "MainGameState/Player";
        private readonly BulletController _bulletController;

        private readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        private readonly PlayerSpaceshipCreator _createPlayerSpaceship;
        private readonly PlayerSpaceshipViewModule _playerSpaceshipViewModule;
        private PlayerSpaceshipData _playerSpaceshipData;
        private PlayerSpaceshipView _playerSpaceshipView;

        public PlayerSpaceshipModule(IAssetBundleLoaderService assetBundleLoaderService, ICameraService cameraService)
        {
            _createPlayerSpaceship = new PlayerSpaceshipCreator(assetBundleLoaderService);
            _playerSpaceshipViewModule = new PlayerSpaceshipViewModule(cameraService);
        }

        public void CreatePlayerSpaceship(string name)
        {
            _playerSpaceshipData = new PlayerSpaceshipData(name);
            var playerSpaceshipView = _createPlayerSpaceship.CreatePlayerSpaceship();
            _playerSpaceshipViewModule.Setup(playerSpaceshipView);
        }

        public void MoveSpaceship(float direction)
        {
            _playerSpaceshipViewModule.MoveSpaceship(direction, _playerSpaceshipData.Speed);
        }

        //private void CreateBullet(AssetBundle playerBundle)
        //{
        //    if (!_model.IsPlayerEnabled)
        //    {
        //        return;
        //    }

        //    var bulletGO =
        //        Object.Instantiate(
        //            _client.AssetBundleLoaderService.TryLoadAssetFromBundle<GameObject>(playerBundle, BULLET_ASSET_NAME));

        //    _bulletController.Setup(bulletGO.GetComponent<BulletView>());
        //}

        //private void CreatePlayer(AssetBundle playerBundle)
        //{
        //    var playerGO =
        //        Object.Instantiate(
        //            _client.AssetBundleLoaderService.TryLoadAssetFromBundle<GameObject>(playerBundle, PLAYER_ASSET_NAME));

        //    _playerView = playerGO.GetComponent<PlayerView>();
        //    _playerView.Setup(_model, OnPlayerCollision, OnDieAnimationEnd);
        //    _playerTransform = _playerView.transform;
        //    _model.PlayerTransform = _playerTransform;
        //    _playerSpaceFromBounds = _playerView.PlayerSpriteRenderer.bounds.size.x / 2;

        //    var playerYPos = _model.PlayerYPosRelativeToScreen;
        //    _playerTransform.position = new Vector3(_model.PlayerStartXPosition, playerYPos, 0);
        //}
        //
        // private void DoShoot()
        // {
        //     if (!_model.IsPlayerEnabled)
        //     {
        //         return;
        //     }
        //
        //     _bulletController.ShootBullet(_playerSpaceshipView.ShootPosition.position);
        // }

        // private void OnPlayerCollision(Collider2D other)
        // {
        //     if (!_model.IsPlayerEnabled || other.gameObject.layer != ENEMY_LAYER)
        //     {
        //         return;
        //     }
        //
        //     _client.Broadcaster.Broadcast(new PlayerLoseEvent());
        // }
    }

    public interface IPlayerSpaceshipModule
    {
        void CreatePlayerSpaceship(string name);
        void MoveSpaceship(float direction);
    }
}