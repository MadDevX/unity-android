using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class GamePanel : UIPanel
{
    [SerializeField]
    private Button _exitButton;
    private ConnectionStateMachine _connManager;
    private UIManager _uiManager;
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(ConnectionStateMachine connManager, UIManager uiManager, GameStateMachine gameStateMachine)
    {
        _connManager = connManager;
        _uiManager = uiManager;
        _gameStateMachine = gameStateMachine;
    }

    private void Awake()
    {
        _gameStateMachine.SubscribeToInit(GameState.Countdown, DisableExit);
        _gameStateMachine.SubscribeToDispose(GameState.Countdown, EnableExit);
        _gameStateMachine.SubscribeToInit(GameState.Finished, DisableExit);
        _gameStateMachine.SubscribeToDispose(GameState.Finished, EnableExit);
    }

    private void OnDestroy()
    {
        _gameStateMachine.UnsubscribeFromInit(GameState.Countdown, DisableExit);
        _gameStateMachine.UnsubscribeFromDispose(GameState.Countdown, EnableExit);
        _gameStateMachine.UnsubscribeFromInit(GameState.Finished, DisableExit);
        _gameStateMachine.UnsubscribeFromDispose(GameState.Finished, EnableExit);
    }

    public void OnExit()
    {
        _connManager.SetState(ConnectionState.Null);
        SwitchPanels();
    }

    private void SwitchPanels()
    {
        HidePanel();
        _uiManager.menuPanel.ShowPanel();
    }

    private void DisableExit(GameStateEventArgs e)
    {
        _exitButton.gameObject.SetActive(false);
    }

    private void EnableExit(GameStateEventArgs e)
    {
        _exitButton.gameObject.SetActive(true);
    }
}
