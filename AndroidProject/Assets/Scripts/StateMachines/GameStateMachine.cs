using System;
using System.Collections.Generic;

namespace Assets.Scripts.StateMachines
{
    public class GameStateEventArgs
    {
        public int playerCount;
        public bool someoneWon;

        public GameStateEventArgs(int playerCount, bool someoneWon = false)
        {
            this.playerCount = playerCount;
            this.someoneWon = someoneWon;
        }
    }

    public class GameStateMachine : EventStateMachine<GameState, GameStateEventArgs>
    {
    }
}
