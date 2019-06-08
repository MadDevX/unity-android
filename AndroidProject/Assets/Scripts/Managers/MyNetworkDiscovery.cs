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
    private LobbyManager _lobbyManager;

    private Coroutine _listenCor = null;

    [Inject]
    public void Construct(ConnectionStateMachine connMachine, LobbyManager lobbyManager)
    {
        _connMachine = connMachine;
        _lobbyManager = lobbyManager;
    }

    private void Awake()
    {
        _connMachine.SubscribeToInit(ConnectionState.Host, StartBroadcast);
        _connMachine.SubscribeToDispose(ConnectionState.Host, StopBroadcast);
    }

    private void OnDestroy()
    {
        _connMachine.UnsubscribeFromInit(ConnectionState.Host, StartBroadcast);
        _connMachine.UnsubscribeFromDispose(ConnectionState.Host, StopBroadcast);
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
