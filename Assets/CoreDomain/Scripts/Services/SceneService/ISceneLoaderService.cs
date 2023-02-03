using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Services.SceneService
{
    public interface ISceneLoaderService
    {
        UniTask<bool> LoadScene(SceneType sceneType);
        UniTask<bool> UnloadScene(SceneType sceneType);
        UniTask<bool> ReloadScene(SceneType sceneType);
        UniTask<bool> ReloadScenes(SceneType[] scenesTypes);

        UniTask<bool> SetupScenes(
            SceneType[] unloadScenesTypes,
            SceneType[] loadScenesTypes,
            ScenesSetupOrderType scenesSetupOrderType = ScenesSetupOrderType.FirstUnload);

        UniTask<bool> SetupScenes(
            SceneType unloadSceneType,
            SceneType loadSceneType,
            ScenesSetupOrderType scenesSetupOrderType = ScenesSetupOrderType.FirstUnload);

        UniTask<bool> LoadScenes(SceneType[] scenesTypes);
        UniTask<bool> UnloadScenes(SceneType[] scenesTypes);
    }
}