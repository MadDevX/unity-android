using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class JoinPanel : UIPanel
{
    public InputField gameNameInputField;
    public InputField nickInputField;

    [SerializeField]
    private Button _findButton;
    [SerializeField]
    private Button _hostButton;

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

    private void Start()
    {
        nickInputField.onValueChanged.AddListener(ValidateNick);
        gameNameInputField.onValueChanged.AddListener(ValidateGame);
    }

    private void OnDestroy()
    {
        nickInputField.onValueChanged.RemoveListener(ValidateNick);
        gameNameInputField.onValueChanged.RemoveListener(ValidateGame);
    }

    private void ValidateNick(string text)
    {
        if(text.Length == 0)
        {
            _hostButton.interactable = false;
            _findButton.interactable = false;
        }
        else
        {
            if (gameNameInputField.text.Length > 0)
            {
                _hostButton.interactable = true;
            }
            _findButton.interactable = true;
        }
    }

    private void ValidateGame(string text)
    {
        if (text.Length == 0)
        {
            _hostButton.interactable = false;
        }
        else if (nickInputField.text.Length > 0)
        {
            _hostButton.interactable = true;
        }
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
        _lobbyManager.GameName = gameNameInputField.text;
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
