using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipViewModule
    {
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new (0f, -0.5f);
        
        private readonly IDeviceScreenService _deviceScreenService;
        private PlayerSpaceshipView _playerSpaceshipView;
        private readonly Vector3 _screenBoundsInWorldSpace;
        private float _playerSpaceFromBounds;

        public PlayerSpaceshipViewModule(IDeviceScreenService deviceScreenService)
        {
            _deviceScreenService = deviceScreenService;
            _screenBoundsInWorldSpace = deviceScreenService.ScreenBoundsInWorldSpace;
        }

        public Vector3 SpaceshipShootPosition => _playerSpaceshipView.ShootPosition;

        public void Setup(PlayerSpaceshipView playerSpaceshipView)
        {
            _playerSpaceshipView = playerSpaceshipView;
            _playerSpaceFromBounds = _playerSpaceshipView.SpriteBounds.size.x * 0.5f;

            var startPosition = _screenBoundsInWorldSpace * RelativeToScreenCenterStartPosition + _deviceScreenService.ScreenCenterPointInWorldSpace;
            _playerSpaceshipView.transform.position = startPosition;
        }
        
        public void MoveSpaceship(float xDirection)
        {
           var playerMoveDelta = xDirection * Time.deltaTime * _playerSpaceshipView.Speed;
           var playerNewXPos = _playerSpaceshipView.transform.position.x + playerMoveDelta;

           if (IsInScreenHorizontalBounds(playerNewXPos, _playerSpaceFromBounds))
           {
               _playerSpaceshipView.MoveToXPosition(playerNewXPos);
           }
        }
        
        private bool IsInScreenHorizontalBounds(float xValue, float spaceKeptFromBounds)
        {
            return -_screenBoundsInWorldSpace.x + spaceKeptFromBounds < xValue && xValue < _screenBoundsInWorldSpace.x - spaceKeptFromBounds;
        }

        public void SetSpaceShipName(string name)
        {
            _playerSpaceshipView.SetPlayerName(name);
        }
    }
}