using CoreDomain.Services;

namespace CoreDomain.GameDomain
{
    public class LevelsService : ILevelsService
    {
        private const string LevelsAssetBundlePath = "coredomain/gamedomain/levels";
        private const string LevelsSettingsAssetName = "LevelsSettings";

        private readonly IAssetBundleLoaderService _assetBundleLoaderService;
        private LevelsScriptableObject _levelsSettings;

        public LevelsService(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public void LoadLevels()
        {
            _levelsSettings = _assetBundleLoaderService.LoadAssetFromBundle<LevelsScriptableObject>(LevelsAssetBundlePath, LevelsSettingsAssetName);
        }

        public int GetLevelsAmount()
        {
            return _levelsSettings.LevelsByOrder.Count;
        }
    }
}