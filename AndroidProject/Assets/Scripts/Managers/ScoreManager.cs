using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ScoreManager : NetworkBehaviour
{
    private Dictionary<NetworkInstanceId, int> _scores = new Dictionary<NetworkInstanceId, int>();
    public event Action OnDictChanged;
    
    private ConnectionStateMachine _connMachine;

    [Inject]
    public void Construct(ConnectionStateMachine connMachine)
    {
        _connMachine = connMachine;
    }

    public List<KeyValuePair<NetworkInstanceId, int>> GetScoreList()
    {
        List<KeyValuePair<NetworkInstanceId, int>> scoreList = new List<KeyValuePair<NetworkInstanceId, int>>();
        foreach(var entry in _scores)
        {
            scoreList.Add(entry);
        }
        return scoreList;
    }

    public void AddPlayer(NetworkInstanceId netId)
    {
        if(_connMachine.State == ConnectionState.Host)
        {
            RpcAddPlayer(netId);
            SyncDictionary();
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

    private void SyncDictionary()
    {
        if (_connMachine.State == ConnectionState.Host)
        {
            foreach (var entry in _scores)
            {
                RpcUpdateEntry(entry.Key, entry.Value);
            }
        }
    }

    [ClientRpc]
    private void RpcAddPlayer(NetworkInstanceId netId)
    {
        _scores.Add(netId, 0);
        Debug.Log($"elo dodano: {_scores.Keys.Count}");
        OnDictChanged?.Invoke();
    }

    [ClientRpc]
    private void RpcRemovePlayer(NetworkInstanceId netId)
    {
        _scores.Remove(netId);
        Debug.Log($"elo zabrano: {_scores.Keys.Count}");
        OnDictChanged?.Invoke();
    }

    [ClientRpc]
    private void RpcIncrementScore(NetworkInstanceId netId, int add)
    {
        _scores[netId] += add;
        Debug.Log($"siema siema, punkciki dla: {netId} sa rowne: {_scores[netId]}");
        OnDictChanged?.Invoke();
    }

    [ClientRpc]
    private void RpcUpdateEntry(NetworkInstanceId netId, int score)
    {
        if(_scores.ContainsKey(netId))
        {
            _scores[netId] = score;
        }
        else
        {
            _scores.Add(netId, score);
        }
        Debug.Log($"elo dodano sync: {_scores.Keys.Count}");
        OnDictChanged?.Invoke();
    }

    private void OnDisable()
    {
        _scores.Clear();
    }
}
