using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public class ConnectionStateManager : StateEventManager<ConnectionState>
    {
        ///Use this to signal that state initialization failed.
        public bool StateError { get; set; }

        /// <summary>
        /// Sets specified state. If any attached initialization method signaled error, manager will set its state to ConnectionState.Null.
        /// </summary>
        /// <param name="state">ConnectionState that will replace previous state.</param>
        /// <returns>Returns true if operation was succesful. False if any attached initialization method signaled error.</returns>
        public new bool SetState(ConnectionState state)
        {
            base.SetState(state);
            if(StateError)
            {
                StateError = false;
                SetState(ConnectionState.Null);
                return false;
            }
            return true;
        }
    }
}
