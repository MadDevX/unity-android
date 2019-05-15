using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class NetworkedGameManager : NetworkBehaviour
{
    private GameStateMachine _gameStateManager;
    private LobbyManager _lobbyManager;
    private ConnectionStateMachine _connManager;

    [Inject]
    public void Construct(GameStateMachine gameStateManager, LobbyManager lobbyManager, ConnectionStateMachine connManager)
    {
        _gameStateManager = gameStateManager;
        _lobbyManager = lobbyManager;
        _connManager = connManager;
    }

    public void StartGame()
    {
        if(_connManager.State == ConnectionState.Server)
        {
            _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(_lobbyManager.playerCount));
        }
        if (_connManager.State == ConnectionState.Host || _connManager.State == ConnectionState.Server)
        {
            RpcStartGame();
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(_lobbyManager.playerCount));
        SetPlayersPositionsRpc();
    }

    private void SetPlayersPositionsRpc()
    {
        var players = _lobbyManager.GetReadyPlayers();
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerRespawn>().Respawn(i);
        }
    }
}
