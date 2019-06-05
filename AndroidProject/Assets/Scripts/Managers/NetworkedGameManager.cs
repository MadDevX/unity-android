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

    public void StartCountdown()
    {
        if (_connManager.State == ConnectionState.Host)
        {
            RpcStartCountdown();
        }
    }

    public void StartGame()
    {
        if (_connManager.State == ConnectionState.Host)
        {
            RpcStartGame();
        }
    }

    public void FinishGame(bool someoneWon)
    {
        if (_connManager.State == ConnectionState.Host)
        {
            RpcFinishGame(someoneWon);
        }
    }

    public void StartLobby()
    {
        if (_connManager.State == ConnectionState.Host)
        {
            RpcStartLobby();
        }
    }

    [ClientRpc]
    private void RpcStartLobby()
    {
        _gameStateManager.SetState(GameState.Lobby, new GameStateEventArgs(_lobbyManager.playerCount));
    }

    [ClientRpc]
    private void RpcStartCountdown()
    {
        _gameStateManager.SetState(GameState.Countdown, new GameStateEventArgs(_lobbyManager.playerCount));
        SetPlayersPositionsRpc();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        _gameStateManager.SetState(GameState.Started, new GameStateEventArgs(_lobbyManager.playerCount));
    }

    [ClientRpc]
    private void RpcFinishGame(bool someoneWon)
    {
        _gameStateManager.SetState(GameState.Finished, new GameStateEventArgs(_lobbyManager.playerCount, someoneWon));
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
