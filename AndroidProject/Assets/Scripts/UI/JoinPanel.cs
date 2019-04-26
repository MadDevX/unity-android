using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JoinPanel : MonoBehaviour
{
    public ExitPanel exitPanel;

    public InputField addressInput;

    private NetworkManager _networkManager;
    private NetworkClient _client;

    private void Start()
    {
        _networkManager = ServiceProvider.Instance.networkManager;
    }

    public void OnJoinButton()
    {
        _networkManager.networkAddress = addressInput.text;
        _client = _networkManager.StartClient();
        if (_client != null)
        {
            SwitchPanels(ConnectionMode.Client);
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
            SwitchPanels(ConnectionMode.Host);
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
            SwitchPanels(ConnectionMode.Server);
        }
        else
        {
            Debug.LogWarning("cannot start server");
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void SwitchPanels(ConnectionMode mode)
    {
        HidePanel();
        exitPanel.ShowPanel(mode);
    }
}
