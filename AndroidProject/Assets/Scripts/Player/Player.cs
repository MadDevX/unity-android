using Assets.Scripts.Managers;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Player : NetworkBehaviour
{
    public EventStateMachine<PlayerStates> stateMachine = new EventStateMachine<PlayerStates>();
    public PlayerState State { private get; set; }
    
    private PlayerMovement _charMovement;
    private PlayerShooting _playerShooting;
    private CinemachineVirtualCamera _vCam;
    private GameStateMachine _gameStateManager;

    [Inject]
    public void Construct(ServiceProvider provider, GameStateMachine gameStateManager)
    {
        _vCam = provider.vCam;
        _gameStateManager = gameStateManager;
    }

    private void Awake()
    {
        _charMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    public override void OnStartLocalPlayer()
    {
        _vCam.Follow = transform;
    }

    private void Start()
    {

        if (!isLocalPlayer) return;
        OnGameJoined();
    }

    private void Update()
    {
        
        if (!isLocalPlayer) return;
        if(stateMachine.State == PlayerStates.Ready && _gameStateManager.State == GameState.Countdown)
        {
            OnGameStarted();
        }
        State.Tick();
    }

    public void Kill()
    {
        if (!isLocalPlayer) return;
        stateMachine.SetState(PlayerStates.Dead);
        Debug.Log("Player was killed!");
    }

    public void TakeDamage()
    {
        stateMachine.SetState(PlayerStates.Dead);
    }

    private void OnGameJoined()
    {
        stateMachine.SetState(PlayerStates.Waiting);
    }

    private void OnGameStarted()
    {
        stateMachine.SetState(PlayerStates.Playing);
    }
}
