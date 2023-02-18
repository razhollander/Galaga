﻿namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public interface IPlayerSpaceshipModule
    {
        void CreatePlayerSpaceship(string name);
        void MoveSpaceship(float direction);
    }
}