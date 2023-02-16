using CoreDomain.Services;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipCreator
    {
        private const string MainGameUiAssetName = "PlayerSpaceship";
        private const string MainGameUiAssetBundlePath = "coredomain/gamedomain/gamestatedomain/maingamedomain/playerspaceship";
        private readonly IAssetBundleLoaderService _assetBundleLoaderService;

        public PlayerSpaceshipCreator(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public PlayerSpaceshipView CreatePlayerSpaceship()
        {
            return _assetBundleLoaderService.InstantiateAssetFromBundle<PlayerSpaceshipView>(MainGameUiAssetBundlePath, MainGameUiAssetName);
        }
    }
}
