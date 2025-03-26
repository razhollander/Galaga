using CoreDomain.Scripts.Extensions;
using UnityEngine;

namespace CoreDomain.Services
{
    public class DeviceScreenService : IDeviceScreenService
    {
        private readonly ICameraService _cameraService;

        public Vector2 ScreenCenterPointInWorldSpace => _cameraService.GetCameraPosition(GameCameraType.World).ToVector2XY();
        public Vector2 ScreenBoundsInWorldSpace => _cameraService.ScreenToWorldPoint(GameCameraType.World, new Vector3(ScreenSizeInScreenSpace.x, ScreenSizeInScreenSpace.y, 0)).ToVector2XY();
        public Vector2 ScreenSizeInScreenSpace => new (Screen.width, Screen.height); // in a normal mobile game i will cache this, because it can't change
        public bool IsInScreenVerticalBounds(float yValue)
        {
            var screenBounds = ScreenBoundsInWorldSpace;
            return -screenBounds.y  < yValue && yValue < screenBounds.y;
        }
        public DeviceScreenService(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }
    }
}