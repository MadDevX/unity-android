using Assets.Scripts.Managers;
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

    private NetworkClient _client;

    private NetworkManager _networkManager;
    private ConnectionModeManager _connManager;
    private UIPanel _exitPanel;

    [Inject]
    public void Construct(ConnectionModeManager connManager, ServiceProvider provider, UIManager uiManager)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
        _exitPanel = uiManager.exitPanel;
    }

    public void OnJoinButton()
    {
        _networkManager.networkAddress = addressInput.text;
        _client = _networkManager.StartClient();
        if (_client != null)
        {
            _connManager.Mode = ConnectionMode.Client;
            SwitchPanels();
        }
        else
        {
            Debug.LogWarning("client cannot connect");
        }
    }

    public void OnHostButton()
    {
        _client = _networkManager.StartHost();
        if (_client != null)
        {
            _connManager.Mode = ConnectionMode.Host;
            SwitchPanels();
        }
        else
        {
            Debug.LogWarning("cannot start host");
        }
    }

    public void OnServerButton()
    {
        if (_networkManager.StartServer())
        {
            _connManager.Mode = ConnectionMode.Server;
            SwitchPanels();
        }
        else
        {
            Debug.LogWarning("cannot start server");
        }
    }

    private void SwitchPanels()
    {
        HidePanel();
        _exitPanel.ShowPanel();
    }
}
