using CoreDomain.Services;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiCreator
    {
        private const string MainGameUiAssetName = "MainGameUiCanvas";
        private const string MainGameUiAssetBundlePath = "coredomain/gamedomain/gamestatedomain/maingamedomain/maingameui";
        private readonly IAssetBundleLoaderService _assetBundleLoaderService;

        public MainGameUiCreator(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public MainGameUiView CreateMainGameUi()
        {
            return _assetBundleLoaderService.InstantiateAssetFromBundle<MainGameUiView>(MainGameUiAssetBundlePath, MainGameUiAssetName);
        }
    }
}