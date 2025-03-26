using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.Utils.Pools;
using CoreDomain.Services;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletPool : AssetFromBundlePool<PlayerBulletView, PlayerBulletPool>
    {
        public PlayerBulletPool(PoolData poolData, DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService) : base(poolData, diContainer, assetBundleLoaderService)
        {
        }

        protected override string AssetBundlePathName => "coredomain/gamedomain/gamestatedomain/maingamedomain/playerbullet";
        public override string AssetName => "PlayerBullet";
        protected override string ParentGameObjectName => "PlayerBullets";
    }
}