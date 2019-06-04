using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ScoreManager : NetworkBehaviour
{
    private Dictionary<NetworkInstanceId, int> _scores = new Dictionary<NetworkInstanceId, int>();

    private LobbyManager _lobbyManager;
    private ConnectionStateMachine _connMachine;

    [Inject]
    public void Construct(LobbyManager lobbyManager, ConnectionStateMachine connMachine)
    {
        _lobbyManager = lobbyManager;
        _connMachine = connMachine;
    }

    public void AddPlayer(NetworkInstanceId netId)
    {
        if(_connMachine.State == ConnectionState.Host)
        {
            RpcAddPlayer(netId);
        }
    }

    public void RemovePlayer(NetworkInstanceId netId)
    {
        if (_connMachine.State == ConnectionState.Host)
        {
            RpcRemovePlayer(netId);
        }
    }

    public void IncrementScore(NetworkInstanceId netId, int add = 1)
    {
        if (_connMachine.State == ConnectionState.Host)
        {
            RpcIncrementScore(netId, add);
        }
    }

    [ClientRpc]
    private void RpcAddPlayer(NetworkInstanceId netId)
    {
        _scores.Add(netId, 0);
        Debug.Log($"elo dodano: {_scores.Keys.Count}");
    }

    [ClientRpc]
    private void RpcRemovePlayer(NetworkInstanceId netId)
    {
        _scores.Remove(netId);
        Debug.Log($"elo zabrano: {_scores.Keys.Count}");
    }

    [ClientRpc]
    private void RpcIncrementScore(NetworkInstanceId netId, int add)
    {
        _scores[netId] += add;
    }

    private void OnDisable()
    {
        _scores.Clear();
    }
}
