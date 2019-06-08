using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class MyNetworkDiscovery : NetworkDiscovery
{
    public event Action<string, string> OnReceivedBroadcastEvent;

    private ConnectionStateMachine _connMachine;
    private GameStateMachine _gameStateMachine;
    private LobbyManager _lobbyManager;

    private Coroutine _listenCor = null;

    [Inject]
    public void Construct(ConnectionStateMachine connMachine, GameStateMachine gameStateMachine, LobbyManager lobbyManager)
    {
        _connMachine = connMachine;
        _gameStateMachine = gameStateMachine;
        _lobbyManager = lobbyManager;
    }

    private void Awake()
    {
        _connMachine.SubscribeToInit(ConnectionState.Host, StartBroadcastChecked);
        _connMachine.SubscribeToDispose(ConnectionState.Host, StopBroadcastChecked);
        _gameStateMachine.SubscribeToInit(GameState.Lobby, StartBroadcastHost);
        _gameStateMachine.SubscribeToDispose(GameState.Lobby, StopBroadcastHost);
    }

    private void OnDestroy()
    {
        _connMachine.UnsubscribeFromInit(ConnectionState.Host, StartBroadcastChecked);
        _connMachine.UnsubscribeFromDispose(ConnectionState.Host, StopBroadcastChecked);
        _gameStateMachine.UnsubscribeFromInit(GameState.Lobby, StartBroadcastHost);
        _gameStateMachine.UnsubscribeFromDispose(GameState.Lobby, StopBroadcastHost);
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        OnReceivedBroadcastEvent?.Invoke(fromAddress, data);
    }

    public void StartBroadcast()
    {
        RefreshBroadcastData();
        Initialize();
        StartAsServer();
    }

    public void StartListening()
    {
        Initialize();
        StartAsClient();
    }

    public void RefreshListen()
    {
        if (_listenCor == null)
        {
            _listenCor = StartCoroutine(RefreshListenCoroutine());
        }
    }

    private void StopBroadcastChecked()
    {
        if(running) StopBroadcast();
    }

    private void StartBroadcastChecked()
    {
        if (!running) StartBroadcast();
    }

    private void StartBroadcastHost(GameStateEventArgs e)
    {
        if (_connMachine.State == ConnectionState.Host && running == false) StartBroadcast();
    }

    private void StopBroadcastHost(GameStateEventArgs e)
    {
        if (_connMachine.State == ConnectionState.Host && running) StopBroadcast();
    }

    private void RefreshBroadcastData()
    {
        broadcastData = _lobbyManager.GameName;
    }

    private IEnumerator RefreshListenCoroutine()
    {
        StopBroadcast();
        yield return new WaitForSeconds(0.2f);
        StartListening();
        _listenCor = null;
    }
}
