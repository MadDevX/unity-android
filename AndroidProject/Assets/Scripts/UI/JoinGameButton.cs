using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class JoinGameButton : MonoBehaviour
{
    [SerializeField]
    private Text _textBoxName;
    [SerializeField]
    private Text _textBoxIP;

    private GameData _gameData;

    private MyNetworkManager _netManager;
    private MyNetworkDiscovery _netDiscovery;
    private ConnectionStateMachine _connMachine;
    private UIManager _uiManager;

    [Inject]
    public void Construct(ServiceProvider provider, ConnectionStateMachine connMachine, UIManager uiManager)
    {
        _netManager = provider.networkManager;
        _netDiscovery = provider.networkDiscovery;
        _connMachine = connMachine;
        _uiManager = uiManager;
    }

    public void InitButton(GameData data)
    {
        _gameData = data;
        _textBoxName.text = _gameData.gameName;
        _textBoxIP.text = _gameData.GetIP();
    }

    public bool IsDataMatching(GameData data)
    {
        return data.address.Equals(_gameData.address) && data.gameName.Equals(_gameData.gameName);
    }

    /// <summary>
    /// Referenced by UI.
    /// </summary>
    public void OnClick()
    {
        _netManager.networkAddress = _gameData.address;
        _netDiscovery.StopBroadcast();
        
        if(_connMachine.SetState(ConnectionState.Client))
        {
            SwitchPanels();
        }
    }

    private void SwitchPanels()
    {
        _uiManager.gamePanel.ShowPanel();
        _uiManager.gameListPanel.HidePanel();
        _uiManager.joinPanel.ShowPanel();
        _uiManager.menuPanel.HidePanel();
        _uiManager.loadingTracker.ShowTracker();
    }

    [System.Serializable]
    public class GameData
    {
        public string gameName;
        public string address;

        public GameData(string name, string address)
        {
            this.gameName = name;
            this.address = address;
        }

        public string GetIP()
        {
            var strings = address.Split(':');
            var ip = strings[strings.Length - 1];
            return ip;
        }

        public override string ToString()
        {
            return $"{gameName} | IP: {GetIP()}";
        }
    }
}