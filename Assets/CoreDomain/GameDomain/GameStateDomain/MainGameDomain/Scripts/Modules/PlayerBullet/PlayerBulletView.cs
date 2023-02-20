using System;
using CoreDomain.Services;
using UnityEngine;
using Zenject;
using IPoolable = CoreDomain.Scripts.Utils.Pools.IPoolable;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet
{
    public class PlayerBulletView : MonoBehaviour, IUpdatable, IPoolable
    {
        [SerializeField] private float _speed;
        
        private Action<PlayerBulletView> _onOutOfScreen;
        private Action<Collider2D> _onHitEnemy;
        private Transform _transform;
        private Action<PlayerBulletView, Collider2D> _onHitWithCollider2D;
        private IDeviceScreenService _deviceScreenService;
        private IUpdateSubscriptionService _updateSubscriptionService;
        public string Id { get; private set; }
        public Action Despawn { get; set; }

        [Inject]
        private void Inject(IUpdateSubscriptionService updateSubscriptionService, IDeviceScreenService deviceScreenService)
        {
            _updateSubscriptionService = updateSubscriptionService;
            _deviceScreenService = deviceScreenService;
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onHitWithCollider2D?.Invoke(this, other);
        }

        private void MoveUp()
        {
            var isAboveTopScreenBound = _deviceScreenService.ScreenBoundsInWorldSpace.y < _transform.position.y;
            
            if (isAboveTopScreenBound)
            {
                _onOutOfScreen?.Invoke(this);
            }
            else
            {
                _transform.Translate(0, _speed * Time.deltaTime, 0);
            }
        }

        public void Setup(string bulletId, Action<PlayerBulletView,Collider2D> onHitWithCollider2D, Action<PlayerBulletView> onOutOfScreen)
        {
            Id = bulletId;
            _onOutOfScreen = onOutOfScreen;
            _onHitWithCollider2D = onHitWithCollider2D;
        }

        public void ManagedUpdate()
        {
            MoveUp();
        }
        
        public void StartMoving(Vector3 bulletStartPosition)
        {
            transform.position = bulletStartPosition;
            _updateSubscriptionService.RegisterUpdatable(this);
        }
        
        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            _updateSubscriptionService.UnregisterUpdatable(this);
        }
    }
}