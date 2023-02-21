using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.Utils.Pools;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletCreator
    {
        private readonly PlayerBulletPool _playerBulletPool;

        public PlayerBulletCreator(PlayerBulletPool.Factory playerBulletPoolFactory)
        {
            _playerBulletPool = playerBulletPoolFactory.Create(new PoolData(15, 5));
            _playerBulletPool.InitPool();
        }

        public PlayerBulletView CreateBullet()
        {
            return _playerBulletPool.Spawn();
        }
    }
}