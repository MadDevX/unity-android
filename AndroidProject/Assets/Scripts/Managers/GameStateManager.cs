using System;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class GameStateManager
    {
        private GameState _state = GameState.Menu;

        private Dictionary<GameState, Action> _stateEvents = new Dictionary<GameState, Action>();

        public GameState State
        {
            get
            {
                return _state;
            }

            set
            {
                GetEvent(value)?.Invoke();
                _state = value;
            }
        }

        public GameStateManager()
        {
            InitStateEventDictionary();
        }

        public void Subscribe(GameState state, Action method)
        {
            var evt = GetEvent(state);
            evt += method;
            SetEvent(state, evt);
        }

        public void Unsubscribe(GameState state, Action method)
        {
            var evt = GetEvent(state);
            evt -= method;
            SetEvent(state, evt);
        }

        private void InitStateEventDictionary()
        {
            foreach (GameState state in Enum.GetValues(typeof(GameState)))
            {
                _stateEvents.Add(state, null);
            }
        }

        private Action GetEvent(GameState state)
        {
            return _stateEvents[state];
        }

        private void SetEvent(GameState state, Action evt)
        {
            _stateEvents[state] = evt;
        }
    }
}
