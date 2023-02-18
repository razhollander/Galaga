using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class MainGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainGameUiModule>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerSpaceshipModule>().AsSingle().NonLazy();
            Container.BindInterfacesTo<EnemiesModule>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerBulletModule>().AsSingle().NonLazy();
            Container.BindFactory<float, JoystickDraggedCommand, JoystickDraggedCommand.Factory>().AsSingle().NonLazy();
            Container.BindFactory<PlayerBulletHitCommandData, PlayerBulletHitCommand, PlayerBulletHitCommand.Factory>().AsSingle().NonLazy();
        }
    }
}