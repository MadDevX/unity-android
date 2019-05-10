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
    [Inject]
    public void Construct(GameStateManager manager, LobbyManager lobbyManager, ConnectionStateManager connManager)
    {
        _gameStateManager = manager;
        _lobbyManager = lobbyManager;
        _connManager = connManager;
    }

    private void Awake()
    {
        _connManager.SubscribeToDispose(ConnectionState.Server, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Host, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Client, ResetGame);
    }

    private void OnDestroy()
    {
        _connManager.UnsubscribeFromDispose(ConnectionState.Server, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Host, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Client, ResetGame);
    }

    private void Update()
    {
        if(_gameStateManager.State != GameState.Countdown && _connManager.State != ConnectionState.Null)
        {
            StartGame();
        }
        if(_gameStateManager.State != GameState.Menu && _connManager.State == ConnectionState.Null)
        {
            ResetGame();
        }
    }

    public void StartGame()
    {
        _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(4));
    }

    public void PauseGame()
    {

    }

    public void ResetGame()
    {
        _gameStateManager.SetState(GameState.Finished, new GameStateEventArgs(4));
        _gameStateManager.SetState(GameState.Menu, new GameStateEventArgs(4));
    }


}
