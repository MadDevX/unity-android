using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerScore : MonoBehaviour
{
    private NetworkIdentity _netIdentity;

    private ConnectionStateMachine _connMachine;
    private ScoreManager _scoreManager;

    [Inject]
    public void Construct(ConnectionStateMachine connMachine, ScoreManager scoreManager)
    {
        _connMachine = connMachine;
        _scoreManager = scoreManager;
    }

    private void Awake()
    {
        _netIdentity = GetComponent<NetworkIdentity>();
    }

    private void Start()
    {
        _scoreManager.AddPlayer(_netIdentity.netId);
    }

    private void OnDestroy()
    {
        _scoreManager.RemovePlayer(_netIdentity.netId);
    }
}
