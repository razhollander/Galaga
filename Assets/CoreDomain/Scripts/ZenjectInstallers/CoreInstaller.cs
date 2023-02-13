using CoreDomain;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using Services.Logs;
using Services.Logs.Base;
using Systems;
using UnityEngine;
using Zenject;

namespace CoreDomain
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private UpdateSubscriptionService _updateSubscriptionService;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CameraService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneLoaderService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<AssetBundleLoaderService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<StateMachineService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UnityLogger>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UpdateSubscriptionService>().FromInstance(_updateSubscriptionService).AsSingle().NonLazy();
            Container.Bind<GameInputActions>().AsSingle().NonLazy();
        }
    }
}
