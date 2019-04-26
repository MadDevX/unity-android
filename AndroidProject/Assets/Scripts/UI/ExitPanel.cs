using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExitPanel : MonoBehaviour
{
    public JoinPanel joinPanel;

    private NetworkManager _networkManager;
    private ConnectionMode _connectionMode;

    private void Start()
    {
        _networkManager = ServiceProvider.Instance.networkManager;
    }

    public void OnExit()
    {
        switch(_connectionMode)
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
        }
        SwitchPanels();
    }

    public void ShowPanel(ConnectionMode mode)
    {
        _connectionMode = mode;
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void SwitchPanels()
    {
        HidePanel();
        joinPanel.ShowPanel();
    }
}
