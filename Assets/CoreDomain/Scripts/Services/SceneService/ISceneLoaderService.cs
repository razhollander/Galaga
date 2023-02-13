using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Services.SceneService
{
    public interface ISceneLoaderService
    {
        UniTask<bool> TryLoadScene(string sceneName);
        UniTask<bool> TryUnloadScene(string sceneName);
        UniTask<bool> TryReloadScene(string sceneName);
        UniTask<bool> TryReloadScenes(string[] scenesNames);
        UniTask<bool> TryLoadScenes(string[] scenesNames);
        UniTask<bool> TryUnloadScenes(string[] scenesNames);
        UniTask<bool> TrySwitchScenes(string unloadSceneName, string loadSceneName);
    }
}