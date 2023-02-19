using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PathCreation;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        private const float RotateAnglesInASecond = 180f;
        //private Action<EnemyView> _onHit;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _isRotationLocked;

        public string Id { get; private set; }
        // public void HitByBullet()
        // {
        //     _onHit(this);
        // }

        // public void Setup(int rowIndex, int columnIndex, Action<EnemyView> onHit)
        // {
        //     _onHit = onHit;
        //     RowIndex = rowIndex;
        //     ColumnIndex = columnIndex;
        // }

        public async UniTask RotateTowardsDirection(Vector3 direction)
        {
            var directionAngles= Quaternion.LookRotation(Vector3.forward, direction).eulerAngles;
            await transform.DORotate(directionAngles, RotateAnglesInASecond).SetSpeedBased(true);
        }

        public async UniTask FollowPath(VertexPath path)
        {
            var distanceAlongPath = 0f;
            var pathLength = path.length;
            
            while (distanceAlongPath<pathLength)
            {
                transform.position = path.GetPointAtDistance(distanceAlongPath);

                if (!_isRotationLocked)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, path.GetDirectionAtDistance(distanceAlongPath));
                }
                
                await UniTask.Yield();
                distanceAlongPath += _moveSpeed * Time.deltaTime;
            }
            
            transform.position = path.GetPoint(path.NumPoints-1);
        }

        public void Setup(string enemyId)
        {
            Id = enemyId;
        }
    }
}