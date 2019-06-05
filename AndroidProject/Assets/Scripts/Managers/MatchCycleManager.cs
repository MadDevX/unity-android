using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.StateMachines;
using Zenject;
using System;

public class MatchCycleManager : MonoBehaviour
{
    private LobbyManager _lobbyManager;
    private GameManager _gameManager;
    private GameStateMachine _gameStateManager;

    public event Action OnAllPlayersDied;

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

    private void Update()
    {
        if (_gameStateManager.State == GameState.Started)
        {
            CheckIfAllDead();
        }
    }

    private void CheckIfGameStarts()
    {               
        if (_gameStateManager.State == GameState.Lobby)
        {
            if (_lobbyManager.playerCount >= 1 && _lobbyManager.playerCount == _lobbyManager.readyPlayerCount)
            {
                _lobbyManager.InitActivePlayers();
                _gameManager.StartCountdown();
            }
        }
    }

    private void CheckIfAllDead()
    {
        foreach (var player in _lobbyManager.GetActivePlayers())
        {
            if (player.stateMachine.State != PlayerStates.Dead) return;
        }
        OnAllPlayersDied?.Invoke();
        _gameManager.FinishGame();
    }
}
