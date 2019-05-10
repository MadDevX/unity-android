using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class MyNetworkManager : NetworkManager
{
    public event Action<int> OnNumPlayersChanged;

    public bool MyStartClient()
    {
        var client = StartClient();
        if (client == null)
        {
            Debug.LogError("client cannot connect");
            return false;
        }
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
}
