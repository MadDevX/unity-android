using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class GameManager : MonoBehaviour
{   
    private GameStateManager _gameStateManager;
    private LobbyManager _lobbyManager;
    private ConnectionStateManager _connManager;
    private NetworkedGameManager _netGameManager;

    [Inject]
    public void Construct(GameStateManager manager, LobbyManager lobbyManager, ConnectionStateManager connManager, NetworkedGameManager netGameManager)
    {
        _gameStateManager = manager;
        _lobbyManager = lobbyManager;
        _connManager = connManager;
        _netGameManager = netGameManager;
    }

    private void Awake()
    {
        _connManager.SubscribeToInit(ConnectionState.Client, StartLobby);
        _connManager.SubscribeToInit(ConnectionState.Host, StartLobby);
        _connManager.SubscribeToInit(ConnectionState.Server, StartLobby);

        _connManager.SubscribeToDispose(ConnectionState.Server, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Host, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Client, ResetGame);
    }

    private void OnDestroy()
    {        
        _connManager.UnsubscribeFromInit(ConnectionState.Client, StartLobby);
        _connManager.UnsubscribeFromInit(ConnectionState.Host, StartLobby);
        _connManager.UnsubscribeFromInit(ConnectionState.Server, StartLobby);

        _connManager.UnsubscribeFromDispose(ConnectionState.Server, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Host, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Client, ResetGame);
    }

    private void Update()
    {
        if(_gameStateManager.State != GameState.Menu && _connManager.State == ConnectionState.Null)
        {
            ResetGame();
        }
    }

    public void StartGame()
    {
        _netGameManager.StartGame();
    }

    private void StartLobby()
    {
        _gameStateManager.SetState(GameState.Lobby, new GameStateEventArgs(_lobbyManager.playerCount));
    }

    public void ResetGame()
    {
        _gameStateManager.SetState(GameState.Finished, new GameStateEventArgs(_lobbyManager.playerCount));
        _gameStateManager.SetState(GameState.Menu, new GameStateEventArgs(_lobbyManager.playerCount));
    }


}
