using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipViewModule
    {
        private readonly ICameraService _cameraService;
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new (0f, -0.5f);
        private PlayerSpaceshipView _playerSpaceshipView;
        private readonly Vector3 _screenBoundsInWorldSpace;
        private float _playerSpaceFromBounds;

        public PlayerSpaceshipViewModule(ICameraService cameraService)
        {
            _cameraService = cameraService;
            _screenBoundsInWorldSpace = _cameraService.ScreenToWorldPoint(GameCameraType.World, new Vector3(Screen.width, Screen.height, 0));
        }
        
        public void Setup(PlayerSpaceshipView playerSpaceshipView)
        {
            _playerSpaceshipView = playerSpaceshipView;
            _playerSpaceFromBounds = _playerSpaceshipView.PlayerSpriteRenderer.bounds.size.x / 2;
            
            var startPosition = _screenBoundsInWorldSpace * RelativeToScreenCenterStartPosition;
            _playerSpaceshipView.transform.position = startPosition;
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
            return -_screenBoundsInWorldSpace.x + spaceKeptFromBounds < xValue && xValue < _screenBoundsInWorldSpace.x - spaceKeptFromBounds;
        }
    }
}