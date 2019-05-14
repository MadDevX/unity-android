using System;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class GameStateEventArgs
    {
        public int playerCount;

        public GameStateEventArgs(int playerCount)
        {
            this.playerCount = playerCount;
        }
    }

    public class GameStateMachine : EventStateMachine<GameState, GameStateEventArgs>
    {
    }
}
