using CoreDomain.Utils.Pools;
using CoreDomain.Services;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class BeeEnemiesPool : AssetFromBundlePool<EnemyView, BeeEnemiesPool>
    {
        public BeeEnemiesPool(PoolData poolData, DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService) : base(poolData, diContainer, assetBundleLoaderService)
        {
        }

        protected override string AssetBundlePathName => "coredomain/gamedomain/gamestatedomain/maingamedomain/enemies";
        public override string AssetName => "EnemyBeeSpaceship";
        protected override string ParentGameObjectName => "BeeEnemiesParent";
    }
}