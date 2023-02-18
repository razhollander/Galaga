using System;
using Client;
using Features.MainGameScreen.Bullet;
using CoreDomain;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet
{
    public class BulletView : MonoBehaviour, IUpdatable
    {
        private Action _onOutOfScreen;
        private Action<Collider2D> _onHitEnemy;
        private bool _didAlreadyHitEnemy; // used when at the same frame the bullet hit multiple enemies
        private BulletModel _model;
        private float _speed;
        private IClient _client;
        private Transform _transform;

        public void OnDestroy()
        {
            RemoveListeners();
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_didAlreadyHitEnemy)
            {
                return;
            }

            _onHitEnemy(other);
        }

        public void ManagedUpdate()
        {
            if (!_model.IsBulletEnabled)
            {
                return;
            }

            _transform.Translate(0, _speed * Time.deltaTime, 0);

            if (!_client.CameraManager.IsInScreenVerticalBounds(_transform.position.y))
            {
                _onOutOfScreen();
            }
        }

        public void Setup(IClient client, BulletModel model, Action<Collider2D> onHitEnemy, Action onOutOfScreen,
            float speed)
        {
            _client = client;
            _onHitEnemy = onHitEnemy;
            _onOutOfScreen = onOutOfScreen;
            _speed = speed;
            _model = model;
        }

        public void StartMoving()
        {
            _didAlreadyHitEnemy = false;
            AddListeners();
        }

        public void StopMoving()
        {
            _didAlreadyHitEnemy = true;
            RemoveListeners();
        }

        private void AddListeners()
        {
            _client.UpdateSubscriptionService.RegisterUpdatable(this);
        }

        private void RemoveListeners()
        {
            _client.UpdateSubscriptionService.UnregisterUpdatable(this);
        }
    }
}