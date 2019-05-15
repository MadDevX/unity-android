using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ExitPanel : UIPanel
{
    private ConnectionStateMachine _connManager;
    private UIPanel _joinPanel;

    [Inject]
    public void Construct(ConnectionStateMachine connManager, UIManager uiManager)
    {
        _connManager = connManager;
        _joinPanel = uiManager.joinPanel;
    }

    public void OnExit()
    {
        _connManager.SetState(ConnectionState.Null);
        SwitchPanels();
    }

    private void SwitchPanels()
    {
        HidePanel();
        _joinPanel.ShowPanel();
    }
}
