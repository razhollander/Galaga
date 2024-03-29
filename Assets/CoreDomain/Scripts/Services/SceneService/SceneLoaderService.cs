﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreDomain.Scripts.Services.SceneService
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly HashSet<string> _loadedScenes = new HashSet<string>();
        private readonly HashSet<string> _loadingScenes = new HashSet<string>();

        public SceneLoaderService()
        {
            AddOpenedScenesToLoadedHashset();
        }

        public async UniTask<bool> TryLoadScene(string sceneName)
        {
            var isSceneAlreadyLoaded = _loadedScenes.Contains(sceneName);

            if (isSceneAlreadyLoaded)
            {
                LogService.LogError($"scene:{sceneName} is already Loaded");
                return false;
            }

            var isSceneAlreadyLoading = _loadingScenes.Contains(sceneName);

            if (isSceneAlreadyLoading)
            {
                LogService.LogError($"scene:{sceneName} is already Loading");
                return false;
            }

            await LoadScene(sceneName);
            return true;
        }

        public async UniTask<bool> TryUnloadScene(string sceneName)
        {
            var isSceneAlreadyLoaded = _loadedScenes.Contains(sceneName);

            if (!isSceneAlreadyLoaded)
            {
                LogService.LogError($"scene:{sceneName} cant be unloaded as it is not Loaded");
                return false;
            }

            var isSceneAlreadyLoading = _loadingScenes.Contains(sceneName);

            if (isSceneAlreadyLoading)
            {
                LogService.LogError($"scene:{sceneName} cant be unloaded as it during Loading");
                return false;
            }

            await UnloadScene(sceneName);
            return true;
        }

        public async UniTask<bool> TryReloadScene(string sceneName)
        {
            return await TryUnloadScene(sceneName) &&
                   await TryLoadScene(sceneName);
        }

        [Obsolete]
        public async UniTask<bool> TryReloadScenes(string[] scenesNames)
        {
            return await TryUnloadScenes(scenesNames) && await TryLoadScenes(scenesNames);
        }

        public async UniTask<bool> TrySwitchScenes(string unloadSceneName, string loadSceneName)
        {
            return await TryUnloadScene(unloadSceneName) && await TryLoadScene(loadSceneName);
        }

        public async UniTask<bool> TryLoadScenes(string[] scenesNames)
        {
            var didLoadAllScenes = true;

            foreach (var scene in scenesNames)
            {
                didLoadAllScenes &= await TryLoadScene(scene);
            }

            return didLoadAllScenes;
        }

        public async UniTask<bool> TryUnloadScenes(string[] scenesNames)
        {
            var result = true;

            foreach (var scene in scenesNames)
            {
                result &= await TryUnloadScene(scene);
            }

            return result;
        }

        private void AddOpenedScenesToLoadedHashset()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;

                if (!_loadedScenes.Contains(sceneName))
                {
                    _loadedScenes.Add(sceneName);
                }
            }
        }

        private async Task LoadScene(string sceneName)
        {
            _loadingScenes.Add(sceneName);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            _loadingScenes.Remove(sceneName);
            _loadedScenes.Add(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        private async Task UnloadScene(string sceneName)
        {
            await SceneManager.UnloadSceneAsync(sceneName);
            _loadedScenes.Remove(sceneName);
        }
    }
}