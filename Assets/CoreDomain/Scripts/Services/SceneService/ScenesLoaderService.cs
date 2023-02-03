using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.Logs.Base;
using UnityEngine.SceneManagement;

namespace CoreDomain.Scripts.Services.SceneService
{
    public class ScenesLoaderService : ISceneLoaderService
    {
        private readonly HashSet<SceneType> _loadedScenes = new();
        private readonly HashSet<SceneType> _loadingScenes = new();

        public ScenesLoaderService()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;
                var sceneType = Enum.Parse<SceneType>(sceneName);
                var isSceneNotInHashSet = !_loadedScenes.Contains(sceneType);

                if (isSceneNotInHashSet)
                {
                    _loadedScenes.Add(sceneType);
                }
            }
        }

        [Obsolete]
        public async UniTask<bool> LoadScene(SceneType sceneType)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            var canLoadScene = !_loadedScenes.Contains(sceneType) && !_loadingScenes.Contains(sceneType);

            if (canLoadScene)
            {
                var sceneName = sceneType.ToString();
                _loadingScenes.Add(sceneType);
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                _loadingScenes.Remove(sceneType);
                _loadedScenes.Add(sceneType);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                return true;
            }
            else
            {
                LogService.LogError($"sceneType:{sceneType} is either Loading or already Loaded");
                return false;
            }
        }

        [Obsolete]
        public async UniTask<bool> UnloadScene(SceneType sceneType)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            var canUnloadScene = _loadedScenes.Contains(sceneType) && !_loadingScenes.Contains(sceneType);

            if (canUnloadScene)
            {
                await SceneManager.UnloadSceneAsync(sceneType.ToString());
                _loadedScenes.Remove(sceneType);
                return true;
            }
            else
            {
                LogService.LogError($"sceneType:{sceneType} cant be unloaded as it is not Loaded");
                return false;
            }
        }

        [Obsolete]
        public async UniTask<bool> ReloadScene(SceneType sceneType)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            return await UnloadScene(sceneType) &&
                   await LoadScene(sceneType);
        }

        [Obsolete]
        public async UniTask<bool> ReloadScenes(SceneType[] scenesTypes)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            return await UnloadScenes(scenesTypes) && await LoadScenes(scenesTypes);
        }

        [Obsolete]
        public async UniTask<bool> SetupScenes(SceneType unloadScenesTypes, SceneType loadScenesTypes, ScenesSetupOrderType scenesSetupOrderType = ScenesSetupOrderType.FirstUnload)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            switch (scenesSetupOrderType)
            {
                case ScenesSetupOrderType.FirstUnload:
                    return await UnloadScene(unloadScenesTypes) && await LoadScene(loadScenesTypes);
                case ScenesSetupOrderType.FirstLoad:
                    return await LoadScene(loadScenesTypes) && await UnloadScene(unloadScenesTypes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scenesSetupOrderType), scenesSetupOrderType, null);
            }
        }

        [Obsolete]
        public async UniTask<bool> SetupScenes(
            SceneType[] unloadScenesTypes,
            SceneType[] loadScenesTypes,
            ScenesSetupOrderType scenesSetupOrderType = ScenesSetupOrderType.FirstUnload)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            switch (scenesSetupOrderType)
            {
                case ScenesSetupOrderType.FirstUnload:
                    return await UnloadScenes(unloadScenesTypes) && await LoadScenes(loadScenesTypes);
                case ScenesSetupOrderType.FirstLoad:
                    return await UnloadScenes(loadScenesTypes) && await LoadScenes(unloadScenesTypes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scenesSetupOrderType), scenesSetupOrderType, null);
            }
        }

        [Obsolete]
        public async UniTask<bool> LoadScenes(SceneType[] scenesTypes)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            var result = true;

            foreach (var scene in scenesTypes)
            {
                result &= await LoadScene(scene);
            }

            return result;
        }

        
        [Obsolete]
        public async UniTask<bool> UnloadScenes(SceneType[] scenesTypes)
        {
            LogService.LogWarning("Obsolete, please not call it");
            return true;
            var result = true;

            foreach (var scene in scenesTypes)
            {
                result &= await LoadScene(scene);
            }

            return result;
        }
    }
}