using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExitPanel : MonoBehaviour
{
    public JoinPanel joinPanel;
    public NetworkManager networkManager;

    private ConnectionMode _connectionMode;

    public void OnExit()
    {
        switch(_connectionMode)
        {
            case ConnectionMode.Client:
                networkManager.StopClient();
                break;
            case ConnectionMode.Host:
                networkManager.StopHost();
                break;
            case ConnectionMode.Server:
                networkManager.StopServer();
                break;
        }
        SwitchPanels();
    }

    public void ShowPanel(ConnectionMode mode)
    {
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
