using UnityEngine;
using Zenject;

namespace CoreDomain.Scripts.Services.CameraService
{
    [RequireComponent(typeof(Camera))]
    public class CameraServiceSubscriber : MonoBehaviour
    {
        [SerializeField] private CameraType _cameraType;
        private ICameraServiceSubscription _cameraServiceSubscription;

        [Inject]
        private void Inject(ICameraServiceSubscription cameraServiceSubscription)
        {
            _cameraServiceSubscription = cameraServiceSubscription;
        }
        
        private void Start()
        {
            var thisCamera = GetComponent<Camera>();
            _cameraServiceSubscription.SubscribeCamera(_cameraType, thisCamera);
        }

        private void OnDestroy()
        {
            _cameraServiceSubscription?.UnsubscribeCamera(_cameraType);
        }
    }
}