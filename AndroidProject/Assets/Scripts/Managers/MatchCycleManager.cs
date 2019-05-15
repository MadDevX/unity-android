using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.StateMachines;
using Zenject;

public class MatchCycleManager : MonoBehaviour
{
    LobbyManager _lobbyManager;
    GameManager _gameManager;
    GameStateMachine _gameStateManager;

    [Inject]
    public void Construct(LobbyManager lobbyManager, GameManager gameManager, GameStateMachine gameStateManager)
    {
        _lobbyManager = lobbyManager;
        _gameManager = gameManager;
        _gameStateManager = gameStateManager;
    }

    private void Awake()
    {
        _lobbyManager.OnReadyPlayerCountChanged += CheckIfGameStarts;
    }

    private void OnDestroy()
    {
        _lobbyManager.OnReadyPlayerCountChanged -= CheckIfGameStarts;
    }

    private void CheckIfGameStarts()
    {               
        if (_gameStateManager.State == GameState.Lobby)
        {
            if (_lobbyManager.playerCount >= 1 && _lobbyManager.playerCount == _lobbyManager.readyPlayerCount)
            {        
                _gameManager.StartGame();
            }
        }
    }
}
