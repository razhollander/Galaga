using UnityEngine;

namespace CoreDomain.Services
{
    public interface ICameraServiceSubscription
    {
        void SubscribeCamera(GameCameraType type, Camera camera);
        void UnsubscribeCamera(GameCameraType type);
    }
}