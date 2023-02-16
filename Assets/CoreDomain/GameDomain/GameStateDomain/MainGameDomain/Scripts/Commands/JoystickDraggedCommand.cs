using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
using CoreDomain.Scripts.Utils.Command;
using Cysharp.Threading.Tasks;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class JoystickDraggedCommand : CommandOneParameter<float, JoystickDraggedCommand>
    {
        private readonly float _dragValue;
        private readonly IPlayerSpaceshipModule _playerSpaceshipModule;

        public JoystickDraggedCommand(float dragValue, IPlayerSpaceshipModule playerSpaceshipModule)
        {
            _dragValue = dragValue;
            _playerSpaceshipModule = playerSpaceshipModule;
        }

        public override async UniTask Execute()
        {
            _playerSpaceshipModule.MoveSpaceship(_dragValue);
        }
    }
}
