using CoreDomain.Scripts.Services.CameraService;
using Services.Logs.Base;
using UnityEngine;
using Zenject;
namespace CoreDomain.Scripts.ZenjectInstallers
{
    public class CoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CameraService>().AsSingle().NonLazy();
            Debug.Log("Injected");
            // Container.Bind<UrlsMockDataConfiguration>().FromScriptableObject(urlsMockDataConfiguration).AsSingle().NonLazy();
            // Container.BindInstance(environmentType);
            // Container.Bind<IBackendEnvironmentSet>().To<BackendEvironmentSet>().AsSingle().NonLazy();
        }
    }
}
