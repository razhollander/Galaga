using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public interface IPlayerBulletModule
    {
        void FireBullet(Vector3 startPosition);
        void DestroyBullet(string bulletId);
        bool IsBulletExist(string bulletId);
        void Dispose();
    }
}