using Zenject;

namespace Assets.Scripts.Managers
{
    public class ConnectionStateManager : StateEventManager<ConnectionState>
    {
        private MyNetworkManager _networkManager;

        [Inject]
        public void Construct(ServiceProvider provider)
        {
            _networkManager = provider.networkManager;
        }

        /// <summary>
        /// Sets specified state. If any attached initialization method signaled error, manager will set its state to ConnectionState.Null.
        /// </summary>
        /// <param name="state">ConnectionState that will replace previous state.</param>
        /// <returns>Returns true if operation was succesful. False if any attached initialization method signaled error.</returns>
        public new bool SetState(ConnectionState state)
        {
            TerminateConnection(State);
            base.SetState(state);
            if (InitConnection(state) == false)
            {
                base.SetState(ConnectionState.Null);
                return false;
            }
            return true;
        }

        private void TerminateConnection(ConnectionState state)
        {
            switch(state)
            {
                case ConnectionState.Client:
                    _networkManager.StopClient();
                    break;
                case ConnectionState.Host:
                    _networkManager.StopHost();
                    break;
                case ConnectionState.Server:
                    _networkManager.StopServer();
                    break;
            }
        }

        private bool InitConnection(ConnectionState state)
        {
            switch (state)
            {
                case ConnectionState.Client:
                    return _networkManager.MyStartClient();
                case ConnectionState.Host:
                    return _networkManager.MyStartHost();
                case ConnectionState.Server:
                    return _networkManager.MyStartServer();
                default:
                    return true;
            }
        }
    }
}
