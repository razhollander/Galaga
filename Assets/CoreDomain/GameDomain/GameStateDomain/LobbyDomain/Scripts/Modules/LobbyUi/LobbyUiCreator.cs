using CoreDomain.Services;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiCreator
    {
        private const string LobbyUiAssetName = "LobbyUiCanvas";
        private const string LobbyUiAssetBundlePath = "CoreDomain/GameDomain/GameStateDomain/LobbyDomain/LobbyUi/LobbyUiCanvas";

        private readonly IAssetBundleLoaderService _assetBundleLoaderService;

        public LobbyUiCreator(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public LobbyUiView CreateLobbyUi()
        {
            return _assetBundleLoaderService.InstantiateAssetFromBundle<LobbyUiView>(LobbyUiAssetBundlePath, LobbyUiAssetName);
        }
    }
}