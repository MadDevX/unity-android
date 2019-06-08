using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class LobbyManager : NetworkBehaviour
{
    public string GameName { get; set; }
    [SyncVar]
    public int playerCount;
    [SyncVar]
    public int readyPlayerCount;
    public event Action OnReadyPlayerCountChanged;

    private List<PlayerNetworkingLobby> _readyPlayers = new List<PlayerNetworkingLobby>();
    private List<Player> _activePlayers = new List<Player>();

    private ConnectionStateMachine _connManager;
    private MyNetworkManager _networkManager;
    private GameStateMachine _gameStateMachine;

    private Coroutine _refreshCor;


    [Inject]
    public void Construct(ConnectionStateMachine connManager, ServiceProvider provider, GameStateMachine gameStateMachine)
    {
        _connManager = connManager;
        _networkManager = provider.networkManager;
        _gameStateMachine = gameStateMachine;
    }

    public void AddReady(PlayerNetworkingLobby player)
    {
        if(_readyPlayers.Contains(player) == false) _readyPlayers.Add(player);
        RefreshReadyPlayers();
    }

    public void RemoveReady(PlayerNetworkingLobby player)
    {
        _readyPlayers.Remove(player);
        RefreshReadyPlayers();
    }

    public List<GameObject> GetReadyPlayers()
    {
        List<GameObject> players = new List<GameObject>();
        foreach(var player in _readyPlayers)
        {
            players.Add(player.gameObject);
        }
        return players;
    }

    public List<Player> GetActivePlayers()
    {
        return _activePlayers;
    }

    private void RefreshReadyPlayers()
    {
        for(int i = _readyPlayers.Count-1; i>=0; i--)
        {
            if (_readyPlayers[i] == null) _readyPlayers.RemoveAt(i);
        }
        readyPlayerCount = _readyPlayers.Count;
        
        OnReadyPlayerCountChanged?.Invoke();

    }

    private void Awake()
    {
        _connManager.SubscribeToInit(ConnectionState.Server, ResetLobby);
        _connManager.SubscribeToInit(ConnectionState.Host, ResetLobby);

        _gameStateMachine.SubscribeToInit(GameState.Lobby, ClearActivePlayers);

        _networkManager.OnNumPlayersChanged += UpdatePlayerCount;
    }

    private void OnEnable()
    {
        _refreshCor = StartCoroutine(RefreshCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_refreshCor);
    }

    private void OnDestroy()
    {
        _connManager.UnsubscribeFromInit(ConnectionState.Server, ResetLobby);
        _connManager.UnsubscribeFromInit(ConnectionState.Host, ResetLobby);

        _gameStateMachine.UnsubscribeFromInit(GameState.Lobby, ClearActivePlayers);

        _networkManager.OnNumPlayersChanged -= UpdatePlayerCount;
    }

    private void ResetLobby()
    {
        playerCount = 0;
        readyPlayerCount = 0;
    }

    private void UpdatePlayerCount(int count)
    {
        if(_connManager.State == ConnectionState.Server || _connManager.State == ConnectionState.Host)
        {
            playerCount = count;
        }
    }

    private IEnumerator RefreshCoroutine()
    {
        while (true)
        {
            if (_connManager.State == ConnectionState.Server || _connManager.State == ConnectionState.Host)
            {
                RefreshReadyPlayers();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void InitActivePlayers()
    {
        foreach (var rp in _readyPlayers)
        {
            _activePlayers.Add(rp.GetComponent<Player>());
        }
    }

    private void ClearActivePlayers(GameStateEventArgs e)
    {
        _activePlayers.Clear();
    }
}
