using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class MyNetworkManager : NetworkManager
{
    public event Action OnNumPlayersChanged;

    private ConnectionStateManager _connManager;

    [Inject]
    public void Construct(ConnectionStateManager connManager)
    {
        _connManager = connManager;
    }

    private void OnEnable()
    {
        _connManager.SubscribeToInit(ConnectionState.Client, MyStartClient);
        _connManager.SubscribeToInit(ConnectionState.Host, MyStartHost);
        _connManager.SubscribeToInit(ConnectionState.Server, MyStartServer);

        _connManager.SubscribeToDispose(ConnectionState.Client, StopClient);
        _connManager.SubscribeToDispose(ConnectionState.Host, StopHost);
        _connManager.SubscribeToDispose(ConnectionState.Server, StopServer);
    }

    private void OnDisable()
    {
        _connManager.UnsubscribeFromInit(ConnectionState.Client, MyStartClient);
        _connManager.UnsubscribeFromInit(ConnectionState.Host, MyStartHost);
        _connManager.UnsubscribeFromInit(ConnectionState.Server, MyStartServer);

        _connManager.UnsubscribeFromDispose(ConnectionState.Client, StopClient);
        _connManager.UnsubscribeFromDispose(ConnectionState.Host, StopHost);
        _connManager.UnsubscribeFromDispose(ConnectionState.Server, StopServer);
    }

    private void MyStartClient()
    {
        var client = StartClient();
        if (client == null)
        {
            _connManager.StateError = true;
            Debug.LogError("client cannot connect");
        }
    }

    private void MyStartServer()
    {
        if(StartServer() == false)
        {
            _connManager.StateError = true;
            Debug.LogError("cannot start server");
        }
    }

    private void MyStartHost()
    {
        var client = StartHost();
        if (client == null)
        {
            _connManager.StateError = true;
            Debug.LogError("cannot start host");
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        OnNumPlayersChanged?.Invoke();
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        OnNumPlayersChanged?.Invoke();
    }
}
