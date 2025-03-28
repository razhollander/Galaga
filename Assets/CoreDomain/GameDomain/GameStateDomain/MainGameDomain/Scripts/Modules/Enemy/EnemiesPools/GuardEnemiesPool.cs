using CoreDomain.Utils.Pools;
using CoreDomain.Services;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class GuardEnemiesPool : AssetFromBundlePool<EnemyView, GuardEnemiesPool>
    {
        public GuardEnemiesPool(PoolData poolData, DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService) : base(poolData, diContainer, assetBundleLoaderService)
        {
        }

        protected override string AssetBundlePathName => "coredomain/gamedomain/gamestatedomain/maingamedomain/enemies";
        public override string AssetName => "EnemyGuardSpaceship";
        protected override string ParentGameObjectName => "GuardEnemiesParent";
    }
}