using UnityEngine;
using Zenject;

namespace CoreDomain.Services
{
    [RequireComponent(typeof(Camera))]
    public class CameraServiceSubscriber : MonoBehaviour
    {
        [SerializeField] private GameCameraType gameCameraType;
        private ICameraServiceSubscription _cameraServiceSubscription;

        [Inject]
        private void Inject(ICameraServiceSubscription cameraServiceSubscription)
        {
            _cameraServiceSubscription = cameraServiceSubscription;
        }
        
        private void Start()
        {
            var thisCamera = GetComponent<Camera>();
            _cameraServiceSubscription.SubscribeCamera(gameCameraType, thisCamera);
        }

        private void OnDestroy()
        {
            _cameraServiceSubscription?.UnsubscribeCamera(gameCameraType);
        }
    }
}