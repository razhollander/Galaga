using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipViewModule
    {
        private readonly ICameraService _cameraService;
        private static readonly Vector2 RelativeToScreenSpaceshipStartPosition = new (0.5f, 0.25f);
        private PlayerSpaceshipView _playerSpaceshipView;
        private readonly Vector2Int _screenBounds;
        private float _playerSpaceFromBounds;

        public PlayerSpaceshipViewModule(ICameraService cameraService)
        {
            _cameraService = cameraService;
            _screenBounds = new Vector2Int(Screen.width, Screen.height);
        }
        
        public void Setup(PlayerSpaceshipView playerSpaceshipView)
        {
            _playerSpaceshipView = playerSpaceshipView;
            var sc = _screenBounds * RelativeToScreenSpaceshipStartPosition;
            var scV3 = (Vector3) sc;
            Debug.Log($"sc: {sc}, scV3: {scV3}");
            _playerSpaceshipView.transform.position = _cameraService.ScreenToWorldPoint(GameCameraType.World, scV3);
        }
        
        public void MoveSpaceship(float direction, float speed)
        {
            var playerMoveDelta = direction * Time.deltaTime * speed;
            var playerNewXPos = _playerSpaceshipView.transform.position.x + playerMoveDelta;

            if (IsInScreenHorizontalBounds(playerNewXPos, _playerSpaceFromBounds))
            {
                _playerSpaceshipView.transform.Translate(playerMoveDelta, 0, 0);
            }
        }
        
        private bool IsInScreenHorizontalBounds(float xValue, float spaceKeptFromBounds)
        {
            var wordXPoint = _cameraService.WorldToScreenPoint(GameCameraType.World, new Vector3(xValue,0,0)).x;
            return wordXPoint > -_screenBounds.x + spaceKeptFromBounds && wordXPoint < _screenBounds.x - spaceKeptFromBounds;
        }
    }
}