using Cysharp.Threading.Tasks;

namespace CoreDomain.Services
{
    public interface ISceneLoaderService
    {
        UniTask<bool> TryLoadScene(string sceneName);
        UniTask<bool> TryUnloadScene(string sceneName);
        UniTask<bool> TryLoadScenes(string[] scenesNames);
        UniTask<bool> TryUnloadScenes(string[] scenesNames);
    }
}