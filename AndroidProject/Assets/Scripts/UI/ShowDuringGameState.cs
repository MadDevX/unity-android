using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShowDuringGameState : MonoBehaviour
{
    public GameState gameState;
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    private void Awake()
    {
        _gameStateMachine.SubscribeToInit(gameState, ShowPanel);
        _gameStateMachine.SubscribeToDispose(gameState, HidePanel);

    }

    private void Start()
    {
        if (_gameStateMachine.State != gameState)
        {
            HidePanel(null);
        }
    }

    private void OnDestroy()
    {
        _gameStateMachine.UnsubscribeFromInit(gameState, ShowPanel);
        _gameStateMachine.UnsubscribeFromDispose(gameState, HidePanel);
    }

    private void ShowPanel(GameStateEventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void HidePanel(GameStateEventArgs e)
    {
        gameObject.SetActive(false);
    }
}
