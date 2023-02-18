using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.Services;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletCreator
    {
        private const string MainGameUiAssetName = "PlayerBullet";
        private const string MainGameUiAssetBundlePath = "coredomain/gamedomain/gamestatedomain/maingamedomain/playerbullet";
        private readonly IAssetBundleLoaderService _assetBundleLoaderService;
        
        public PlayerBulletCreator(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public PlayerBulletView CreateBullet()
        {
            return _assetBundleLoaderService.InstantiateAssetFromBundle<PlayerBulletView>(MainGameUiAssetBundlePath, MainGameUiAssetName);
        }
    }
}