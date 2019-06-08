using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class JoinPanel : UIPanel
{
    public InputField _gameNameInputField;

    private NetworkManager _networkManager;
    private LobbyManager _lobbyManager;

    private ConnectionStateMachine _connManager;
    private UIManager _uiManager;

    [Inject]
    public void Construct(ConnectionStateMachine connManager, ServiceProvider provider, UIManager uiManager, LobbyManager lobbyManager)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
        _uiManager = uiManager;
        _lobbyManager = lobbyManager;
    }

    public void OnJoinButton()
    {
        //_networkManager.networkAddress = addressInput.text;
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
        _lobbyManager.GameName = _gameNameInputField.text;
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
        _uiManager.menuPanel.HidePanel();
        _uiManager.gamePanel.ShowPanel();
    }
}
