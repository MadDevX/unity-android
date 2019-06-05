﻿using Assets.Scripts.StateMachines;
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
    private GameStateMachine _gameStateMachine;
    private ServiceProvider _serviceProvider;

    [Inject]
    public void Construct(ServiceProvider provider, GameStateMachine gameStateMachine)
    {
        _vCam = provider.vCam;
        _gameStateMachine = gameStateMachine;
        _serviceProvider = provider;
    }

    private void Awake()
    {
        _charMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    public override void OnStartLocalPlayer()
    {
        _serviceProvider.player = this;
        _vCam.Follow = transform;
    }

    private void Start()
    {
        if (!isLocalPlayer) return;
        OnGameJoined();
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (stateMachine.State == PlayerStates.Ready && _gameStateMachine.State == GameState.Countdown)
        {
            OnCountdownStarted();
        }
        if (State != null)
        {
            State.Tick();
        }
        else
        {
            Debug.Log("PlayerState is null!");
        }
    }

    public void Kill()
    {
        if (!isLocalPlayer) return;
        //stateMachine.SetState(PlayerStates.Dead);
        SetPlayerState(PlayerStates.Dead);
        Debug.Log("Player was killed!");
    }

    public void TakeDamage()
    {
        if (!isLocalPlayer) return; //check if it is necessary
        //stateMachine.SetState(PlayerStates.Dead);
        SetPlayerState(PlayerStates.Dead);
    }

    private void OnGameJoined()
    {
        //stateMachine.SetState(PlayerStates.Waiting);
        SetPlayerState(PlayerStates.Waiting);
    }

    private void OnCountdownStarted()
    {
        //stateMachine.SetState(PlayerStates.Playing);
        SetPlayerState(PlayerStates.Playing);
    }

    public void SetPlayerState(PlayerStates state)
    {
        CmdSetPlayerState(state);
    }

    [Command]
    private void CmdSetPlayerState(PlayerStates state)
    {
        RpcSetPlayerState(state);
    }

    [ClientRpc]
    private void RpcSetPlayerState(PlayerStates state)
    {
        stateMachine.SetState(state);
    }
}
