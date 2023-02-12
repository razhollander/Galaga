using System.Collections;
using System.Collections.Generic;
using CoreDomain.Scripts.Utils.Command;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.Services
{
    public class StateMachineService
    {
        // private IGameState CurrentStateObject { get; set; }
        // public void SwitchState(IGameState newState)
        // {
        //     CurrentStateObject.ExitState(exitingState => OnStateExitDone(exitingState, newState));
        // }
        //
        // public void SetupInitialGameState(IGameState initialState)
        // {
        //     CurrentStateObject = initialState;
        //     CurrentStateObject.EnterState();
        // }
        //
        // private void OnStateExitDone(IGameState exitingState, IGameState enteringState)
        // {
        //     // Client.Broadcaster.Broadcast(new OnStateExit {State = CurrentStateObject});
        //     CurrentStateObject = enteringState;
        //
        //     CurrentStateObject.EnterState();
        //     // Client.Broadcaster.Broadcast(new OnStateEnter {State = enteringState});
        // }
        //
        // public class OnStateChangedCommand : Command<OnStateChangedCommand>
        // {
        //     #region --- Members ---
        //
        //     public IGameState State;
        //
        //     #endregion
        //
        //     public override UniTask Execute()
        //     {
        //         throw new System.NotImplementedException();
        //     }
        // }
        //
        // public class OnStateEnter : OnStateChangedEvent
        // {
        // }
        //
        // public class OnStateExit : OnStateChangedEvent
        // {
        // }

    }
}