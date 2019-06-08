using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class GamePanel : UIPanel
{
    private ConnectionStateMachine _connManager;
    private UIManager _uiManager;

    [Inject]
    public void Construct(ConnectionStateMachine connManager, UIManager uiManager)
    {
        _connManager = connManager;
        _uiManager = uiManager;
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
}
