using UnityEngine;

namespace CoreDomain.Services
{
    public interface IDeviceScreenService
    {
        Vector2 ScreenBoundsInWorldSpace { get; }
        Vector2 ScreenCenterPointInWorldSpace { get; }
        Vector2 ScreenSize { get; }
        bool IsInScreenVerticalBounds(float yValue);
    }
}