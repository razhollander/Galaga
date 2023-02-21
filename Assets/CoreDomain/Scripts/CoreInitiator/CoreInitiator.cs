using CoreDomain.Services;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameInitiator
{
    public class CoreInitiator : MonoBehaviour
    {
        private GameInputActions _gameInputActions;
        private ISceneLoaderService _sceneLoaderService;

        [Inject]
        private void Setup(GameInputActions gameInputActions, ISceneLoaderService sceneLoaderService)
        {
            _gameInputActions = gameInputActions;
            _sceneLoaderService = sceneLoaderService;
            
            InitializeServices();
        }

        private void Start()
        {
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            _sceneLoaderService.TryLoadScene(SceneName.Game);
        }

        private void InitializeServices()
        {
            _gameInputActions.Enable();
        }
    }
}