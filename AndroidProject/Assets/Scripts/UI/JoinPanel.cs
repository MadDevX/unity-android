using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class JoinPanel : UIPanel
{
    [SerializeField]
    private InputField addressInput;

    private NetworkManager _networkManager;

    private ConnectionStateMachine _connManager;
    private UIPanel _exitPanel;

    [Inject]
    public void Construct(ConnectionStateMachine connManager, ServiceProvider provider, UIManager uiManager)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
        _exitPanel = uiManager.exitPanel;
    }

    public void OnJoinButton()
    {
        _networkManager.networkAddress = addressInput.text;
        if (_connManager.SetState(ConnectionState.Client))
        {
            SwitchPanels();
        }
        else
        {
            //Show warning
        }
    }

    public void OnHostButton()
    {
        if(_connManager.SetState(ConnectionState.Host))
        {
            SwitchPanels();
        }
        else
        {
            //Show warning
        }
    }

    public void OnServerButton()
    {
        if(_connManager.SetState(ConnectionState.Server))
        {
            SwitchPanels();
        }
        else
        {
            //Show warning
        }
    }

    private void SwitchPanels()
    {
        HidePanel();
        _exitPanel.ShowPanel();
    }
}
