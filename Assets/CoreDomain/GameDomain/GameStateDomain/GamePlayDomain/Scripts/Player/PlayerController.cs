using System;
using Systems;
using Client;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using Features.MainGameScreen.Bullet;
using Features.MainGameScreen.GameLogicManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.MainGameScreen.Player
{
    public class PlayerController : IDisposable
    {
        #region --- Constants ---

        private const string BULLET_ASSET_NAME = "PlayerBullet";
        private const string PLAYER_ASSET_NAME = "PlayerSpaceship";
        private const string PLAYER_BUNDLE_PATH = "MainGameState/Player";

        #endregion


        #region --- Members ---

        private readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        private readonly BulletController _bulletController;
        private float _playerSpaceFromBounds;
        private readonly IClient _client;

        private readonly PlayerInputs _playerInputs;

        private readonly PlayerModel _model;
        private PlayerView _playerView;
        private Transform _playerTransform;

        #endregion


        #region --- Construction ---

        public PlayerController(IClient client)
        {
            _client = client;
            _model = new PlayerModel(_client);
            _bulletController = new BulletController(_client);
            _playerInputs = new PlayerInputs(_client);
            AddListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            RemoveListeners();
            _model?.Dispose();
            _playerInputs?.Dispose();
            _bulletController?.Dispose();

            DestroyAssets();
        }

       // public void Setup()
       // {
       //     var playerBundle = _client.AssetBundleLoaderService.LoadBundle(PLAYER_BUNDLE_PATH);
       //
       //     CreatePlayer(playerBundle);
       //     CreateBullet(playerBundle);
       //
       //     _client.AssetBundleLoaderService.UnloadAssetBundle(playerBundle);
       // }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _playerInputs.MoveChangeEvent += OnPlayerMove;
            _playerInputs.ShootEvent += DoShoot;
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

        private void DestroyAssets()
        {
            Object.Destroy(_playerView.gameObject);
        }

        private void RemoveListeners()
        {
            _playerInputs.MoveChangeEvent -= OnPlayerMove;
            _playerInputs.ShootEvent -= DoShoot;
        }

        #endregion


        #region --- Event Handler ---

        private void DoShoot()
        {
            if (!_model.IsPlayerEnabled)
            {
                return;
            }

            _bulletController.ShootBullet(_playerView.ShootPosition.position);
        }

        private void OnDieAnimationEnd()
        {
            _client.Broadcaster.Broadcast(new PlayerDieEndEvent());
        }

        private void OnPlayerCollision(Collider2D other)
        {
            if (!_model.IsPlayerEnabled || other.gameObject.layer != ENEMY_LAYER)
            {
                return;
            }

            _client.Broadcaster.Broadcast(new PlayerLoseEvent());
        }

        private void OnPlayerMove(float dir)
        {
            if (!_model.IsPlayerEnabled || _playerTransform == null)
            {
                return;
            }

            var playerMoveDelta = dir * Time.deltaTime * _model.PlayerSpeed;
            var playerNewXPos = _playerTransform.position.x + playerMoveDelta;

            if (_client.CameraManager.IsInScreenHorizontalBounds(playerNewXPos, _playerSpaceFromBounds))
            {
                _playerTransform.Translate(playerMoveDelta, 0, 0);
            }
        }

        #endregion
    }
}