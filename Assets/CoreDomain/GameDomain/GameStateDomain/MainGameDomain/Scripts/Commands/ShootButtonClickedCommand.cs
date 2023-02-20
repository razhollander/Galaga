using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
using CoreDomain.Scripts.Utils.Command;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class ShootButtonClickedCommand : CommandSync<ShootButtonClickedCommand>
    {
        private readonly IPlayerSpaceshipModule _playerSpaceshipModule;
        private readonly IPlayerBulletModule _playerBulletModule;

        private ShootButtonClickedCommand(IPlayerSpaceshipModule playerSpaceshipModule, IPlayerBulletModule playerBulletModule)
        {
            _playerSpaceshipModule = playerSpaceshipModule;
            _playerBulletModule = playerBulletModule;
        }

        public override void Execute()
        {
            var spaceShipGunPosition = _playerSpaceshipModule.SpaceShipShootPosition;
            _playerBulletModule.FireBullet(spaceShipGunPosition);
        }
    }
}