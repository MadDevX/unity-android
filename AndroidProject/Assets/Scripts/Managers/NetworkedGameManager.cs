using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class NetworkedGameManager : NetworkBehaviour
{
    private GameStateManager _gameStateManager;
    private LobbyManager _lobbyManager;
    private ConnectionStateManager _connManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager, LobbyManager lobbyManager, ConnectionStateManager connManager)
    {
        _gameStateManager = gameStateManager;
        _lobbyManager = lobbyManager;
        _connManager = connManager;
    }

    public void StartGame()
    {
        if (_connManager.State == ConnectionState.Host)
        {
            RpcStartGame();
        }
        if (_connManager.State == ConnectionState.Server)
        {
            _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(_lobbyManager.playerCount));
            RpcStartGame();
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(_lobbyManager.playerCount));
    }
}
