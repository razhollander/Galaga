using System;
using System.Collections.Generic;
using CoreDomain.Scripts.Extensions;
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
            AddOpenedScenesToLoadedHashset();
        }

        private void AddOpenedScenesToLoadedHashset()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;

                if (sceneName.TryToEnum<SceneType>(out var sceneType) &&
                    !_loadedScenes.Contains(sceneType))
                {
                    _loadedScenes.Add(sceneType);
                }
            }
        }

        public async UniTask<bool> TryLoadScene(SceneType sceneType)
        {
            var isSceneNotAlreadyLoaded = !_loadedScenes.Contains(sceneType) && !_loadingScenes.Contains(sceneType);

            if (isSceneNotAlreadyLoaded)
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
                   await TryLoadScene(sceneType);
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
                    return await UnloadScene(unloadScenesTypes) && await TryLoadScene(loadScenesTypes);
                case ScenesSetupOrderType.FirstLoad:
                    return await TryLoadScene(loadScenesTypes) && await UnloadScene(unloadScenesTypes);
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
                result &= await TryLoadScene(scene);
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
                result &= await TryLoadScene(scene);
            }

            return result;
        }
    }
}