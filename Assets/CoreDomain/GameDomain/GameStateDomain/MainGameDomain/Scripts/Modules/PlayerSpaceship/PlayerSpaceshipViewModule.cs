using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipViewModule
    {
        private readonly IDeviceScreenService _deviceScreenService;
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new (0f, -0.5f);
        
        private PlayerSpaceshipView _playerSpaceshipView;
        private readonly Vector3 _screenBoundsInWorldSpace;
        private float _playerSpaceFromBounds;

        public PlayerSpaceshipViewModule(IDeviceScreenService deviceScreenService)
        {
            _deviceScreenService = deviceScreenService;
            _screenBoundsInWorldSpace = deviceScreenService.ScreenBoundsInWorldSpace;
        }
        
        public void Setup(PlayerSpaceshipView playerSpaceshipView)
        {
            _playerSpaceshipView = playerSpaceshipView;
            _playerSpaceFromBounds = _playerSpaceshipView.PlayerSpriteRenderer.bounds.size.x * 0.5f;

            var startPosition = _screenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + _deviceScreenService.ScreenCenterPointInWorldSpace;
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