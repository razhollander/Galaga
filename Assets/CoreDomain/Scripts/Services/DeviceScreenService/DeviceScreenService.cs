using Extensions;
using UnityEngine;

namespace CoreDomain.Services
{
    public class DeviceScreenService : IDeviceScreenService
    {
        private readonly ICameraService _cameraService;

        public Vector2 ScreenCenterPointInWorldSpace => _cameraService.GetCameraPosition(GameCameraType.World).ToVector2XY();
        public Vector2 ScreenBoundsInWorldSpace => _cameraService.ScreenToWorldPoint(GameCameraType.World, new Vector3(ScreenSize.x, ScreenSize.y, 0)).ToVector2XY();
        public Vector2 ScreenSize { get; }
        public bool IsInScreenVerticalBounds(float yValue)
        {
            var screenBounds = ScreenBoundsInWorldSpace;
            return -screenBounds.y  < yValue && yValue < screenBounds.y;
        }
        public DeviceScreenService(ICameraService cameraService)
        {
            _cameraService = cameraService;
            ScreenSize = new Vector2(Screen.width, Screen.height);
        }
    }
}