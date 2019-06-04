using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class MyNetworkManager : NetworkManager
{
    public event Action<int> OnNumPlayersChanged;

    private GameStateMachine _gameStateMachine;
    private NetworkClient _client;

    public bool ClientIsConnected
    {
        get
        {
            if (_client != null)
                return _client.isConnected;
            else
                return false;
        }
    }

    [Inject]
    public void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public bool MyStartClient()
    {
        _client = StartClient();
        return true;
    }

    public bool MyStartServer()
    {
        if(StartServer() == false)
        {
            Debug.LogError("cannot start server");
            return false;
        }
        return true;
    }

    public bool MyStartHost()
    {
        var client = StartHost();
        if (client == null)
        {
            Debug.LogError("cannot start host");
            return false;
        }
        return true;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        OnNumPlayersChanged?.Invoke(numPlayers);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        OnNumPlayersChanged?.Invoke(numPlayers);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        OnNumPlayersChanged?.Invoke(numPlayers);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        OnNumPlayersChanged?.Invoke(numPlayers);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (_gameStateMachine.State == GameState.Lobby)
        {
            base.OnServerConnect(conn);
        }
        else
        {
            conn.Disconnect();
        }
    }
}
