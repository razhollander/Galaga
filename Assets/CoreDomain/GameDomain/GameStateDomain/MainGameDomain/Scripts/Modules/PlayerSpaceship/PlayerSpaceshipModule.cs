using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipModule : IPlayerSpaceshipModule
    {
        private readonly PlayerSpaceshipCreator _createPlayerSpaceship;
        private readonly PlayerSpaceshipViewModule _playerSpaceshipViewModule;
        private PlayerSpaceshipData _playerSpaceshipData;

        public Vector3 SpaceShipShootPosition => _playerSpaceshipViewModule.SpaceshipShootPosition;

        public PlayerSpaceshipModule(IAssetBundleLoaderService assetBundleLoaderService, IDeviceScreenService deviceScreenService)
        {
            _createPlayerSpaceship = new PlayerSpaceshipCreator(assetBundleLoaderService);
            _playerSpaceshipViewModule = new PlayerSpaceshipViewModule(deviceScreenService);
        }

        public void CreatePlayerSpaceship(string name)
        {
            _playerSpaceshipData = new PlayerSpaceshipData(name);
            var playerSpaceshipView = _createPlayerSpaceship.CreatePlayerSpaceship();
            _playerSpaceshipViewModule.Setup(playerSpaceshipView);
            _playerSpaceshipViewModule.SetSpaceShipName(_playerSpaceshipData.Name);
        }

        public void Dispose()
        {
            _playerSpaceshipViewModule.DestroySpaceSip();
        }

        public void MoveSpaceship(float xDirection)
        {
            _playerSpaceshipViewModule.MoveSpaceship(xDirection);
        }
    }
}