using GameStates;
using CoreDomain;
using CoreDomain.Services;
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
            //Container.BindInterfacesTo<GameSaverService>().AsSingle().NonLazy();
            //Container.BindInterfacesTo<BroadcastService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<AssetBundleSystem>().AsSingle().NonLazy();
            //Container.BindInterfacesTo<PopupsManager>().AsSingle().NonLazy();
            //Container.BindInterfacesTo<StateMachine>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UnityLogger>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UpdateSubscriptionService>().FromInstance(_updateSubscriptionService).AsSingle().NonLazy();
            Container.Bind<GameInputActions>().AsSingle().NonLazy();

            // Container.Bind<UrlsMockDataConfiguration>().FromScriptableObject(urlsMockDataConfiguration).AsSingle().NonLazy();
            // Container.BindInstance(environmentType);
            // Container.Bind<IBackendEnvironmentSet>().To<BackendEvironmentSet>().AsSingle().NonLazy();
        }
    }
}
