using UnityEngine;

namespace CoreDomain.Services
{
    public interface IDeviceScreenService
    {
        Vector2 ScreenBoundsInWorldSpace { get; }
        Vector2 ScreenCenterPointInWorldSpace { get; }
        Vector2 ScreenSizeInScreenSpace { get; }
        bool IsInScreenVerticalBounds(float yValue);
    }
}