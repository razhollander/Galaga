using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class PlayerBulletHitCommandData
    {
        public readonly Collider2D HitCollider2D;
        public readonly PlayerBulletView HitPlayerBulletView;

        public PlayerBulletHitCommandData(Collider2D hitCollider2D, PlayerBulletView hitPlayerBulletView)
        {
            HitCollider2D = hitCollider2D;
            HitPlayerBulletView = hitPlayerBulletView;
        }
    }
}