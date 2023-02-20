using System;
using System.Threading;
using CoreDomain.Scripts.Utils.Pools;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PathCreation;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemyView : MonoBehaviour, IPoolable
    {
        private const float RotateAnglesInASecond = 180f;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _isRotationLocked;
        private Transform _transform;
        private CancellationTokenSource _whileAliveCancellationToken;
        public Action Despawn { get; set; }

        private void Awake()
        {
            _transform = transform;
        }
        public string Id { get; private set; }

        public async UniTask RotateTowardsDirection(Vector3 direction)
        {
            var directionAngles= Quaternion.LookRotation(Vector3.forward, direction).eulerAngles;
            await _transform.DORotate(directionAngles, RotateAnglesInASecond).SetSpeedBased(true).WithCancellation(_whileAliveCancellationToken.Token);
        }

        public async UniTask FollowPath(VertexPath path, Func<Vector3> GetAddedDeltaToPath)
        {
            var distanceAlongPath = 0f;
            var pathLength = path.length;
            
            while (distanceAlongPath<pathLength)
            {
                if (_transform == null)
                {
                    Debug.Log("Transform null");
                }

                _transform.position = path.GetPointAtDistance(distanceAlongPath)+GetAddedDeltaToPath();
                
                if (!_isRotationLocked)
                {
                    _transform.rotation = Quaternion.LookRotation(Vector3.forward, path.GetDirectionAtDistance(distanceAlongPath));
                }
                
                await UniTask.Yield(_whileAliveCancellationToken.Token);
                distanceAlongPath += _moveSpeed * Time.deltaTime;
            }
            
            _transform.position = path.GetPoint(path.NumPoints-1)+GetAddedDeltaToPath();
        }

        public void Setup(string enemyId)
        {
            Id = enemyId;
        }
        
        public void OnSpawned()
        {
            _whileAliveCancellationToken = new CancellationTokenSource();
        }

        public void OnDespawned()
        {
            _whileAliveCancellationToken.Cancel();
            gameObject.SetActive(false);
        }
    }
}