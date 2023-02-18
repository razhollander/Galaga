using CoreDomain.Services;

namespace CoreDomain.GameDomain
{
    public class LevelsService : ILevelsService
    {
        private const string LevelsAssetBundlePath = "coredomain/gamedomain/levels";
        private const string LevelsSettingsAssetName = "LevelsSettings";

        private readonly IAssetBundleLoaderService _assetBundleLoaderService;
        private LevelsData _levelsData;

        public LevelsService(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public void LoadLevels()
        {
            _levelsData = _assetBundleLoaderService.LoadScriptableObjectAssetFromBundle<LevelsData>(LevelsAssetBundlePath, LevelsSettingsAssetName);
        }

        public int GetLevelsAmount()
        {
            return _levelsData.LevelsByOrder.Length;
        }

        public LevelData GetLevel(int levelNumber)
        {
            return _levelsData.LevelsByOrder[levelNumber - 1];
        }
    }
}