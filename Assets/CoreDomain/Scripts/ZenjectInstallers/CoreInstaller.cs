using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoreInstaller : Installer
{
    public override void InstallBindings()
    {
        // Container.BindInterfacesTo<PopupService>().AsSingle().NonLazy();
        // Container.Bind<UrlsMockDataConfiguration>().FromScriptableObject(urlsMockDataConfiguration).AsSingle().NonLazy();
        // Container.BindInstance(environmentType);
        // Container.Bind<IBackendEnvironmentSet>().To<BackendEvironmentSet>().AsSingle().NonLazy();
    }
}
