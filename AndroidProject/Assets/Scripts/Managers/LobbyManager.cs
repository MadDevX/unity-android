using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class LobbyManager : NetworkBehaviour
{
    [SyncVar]
    public int playerCount;
    [SyncVar]
    public int readyPlayerCount;

    public List<PlayerNetworkingLobby> readyPlayers = new List<PlayerNetworkingLobby>();

    private ConnectionStateManager _connManager;
    private MyNetworkManager _networkManager;

    [Inject]
    public void Construct(ConnectionStateManager connManager, ServiceProvider provider)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
    }

    public void AddReady(PlayerNetworkingLobby player)
    {
        RefreshReadyPlayers();
        if(readyPlayers.Contains(player) == false) readyPlayers.Add(player);
        readyPlayerCount = readyPlayers.Count;
    }

    public void RemoveReady(PlayerNetworkingLobby player)
    {
        RefreshReadyPlayers();
        readyPlayers.Remove(player);
        readyPlayerCount = readyPlayers.Count;
    }

    private void RefreshReadyPlayers()
    {
        for(int i = readyPlayers.Count-1; i>=0; i--)
        {
            if (readyPlayers[i] == null) readyPlayers.RemoveAt(i);
        }
    }

    private void Awake()
    {
        _connManager.SubscribeToInit(ConnectionState.Server, ResetLobby);
        _connManager.SubscribeToInit(ConnectionState.Host, ResetLobby);

        _networkManager.OnNumPlayersChanged += UpdatePlayerCount;
    }

    private void OnDestroy()
    {
        _connManager.UnsubscribeFromInit(ConnectionState.Server, ResetLobby);
        _connManager.UnsubscribeFromInit(ConnectionState.Host, ResetLobby);

        _networkManager.OnNumPlayersChanged -= UpdatePlayerCount;
    }

    private void ResetLobby()
    {
        playerCount = 0;
        readyPlayerCount = 0;
    }

    private void UpdatePlayerCount(int count)
    {
        if(_connManager.State == ConnectionState.Server || _connManager.State == ConnectionState.Host)
        {
            playerCount = count;
        }
    }
}
