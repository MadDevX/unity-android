using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ExitPanel : UIPanel
{
    private NetworkManager _networkManager;
    private ConnectionModeManager _connManager;
    private UIPanel _joinPanel;

    [Inject]
    public void Construct(ServiceProvider provider, ConnectionModeManager connManager, UIManager uiManager)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
        _joinPanel = uiManager.joinPanel;
    }

    public void OnExit()
    {
        switch(_connManager.Mode)
        {
            case ConnectionMode.Client:
                _networkManager.StopClient();
                break;
            case ConnectionMode.Host:
                _networkManager.StopHost();
                break;
            case ConnectionMode.Server:
                _networkManager.StopServer();
                break;
            case ConnectionMode.Null:
                Debug.LogError("Connection mode was set to Null.");
                break;
        }
        _connManager.Mode = ConnectionMode.Null;
        SwitchPanels();
    }

    private void SwitchPanels()
    {
        HidePanel();
        _joinPanel.ShowPanel();
    }
}
