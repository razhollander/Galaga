using System.Threading.Tasks;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.Services.GameStates
{
     public class MainGameState : BaseGameState<MainGameStateEnterData>
     {
         private ISceneLoaderService _sceneLoaderService;

         public override GameStateType GameState => GameStateType.MainGame;
    
         public MainGameState(ISceneLoaderService sceneLoaderService, MainGameStateEnterData gameStateEnterData) : base(gameStateEnterData)
         {
             _sceneLoaderService = sceneLoaderService;
         }

         public override async UniTask EnterState()
         {
             await _sceneLoaderService.TryLoadScene(SceneName.MainGame);
             await StartStateInitiator();
         }
         
         private async UniTask StartStateInitiator()
         {
             await GameObject.FindObjectOfType<MainGameInitiator>().StartState(EnterData);
         }
    
         public override async UniTask ExitState()
         {
             await DisposeStateInitiator();
             await _sceneLoaderService.TryUnloadScene(SceneName.MainGame);
         }

         private async Task DisposeStateInitiator()
         {
             await GameObject.FindObjectOfType<MainGameInitiator>().DisposeState(EnterData);
         }

         public class Factory : PlaceholderFactory<MainGameStateEnterData, MainGameState>
         {
         }
    }
}