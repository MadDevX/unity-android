using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class GameManager : NetworkBehaviour
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
        _connManager.SubscribeToInit(ConnectionState.Null, ResetGame);
    }

    private void OnDestroy()
    {
        _connManager.UnsubscribeFromInit(ConnectionState.Null, ResetGame);
    }

    public override void OnStartServer()
    {
        StartGame();
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            StartGame();
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
