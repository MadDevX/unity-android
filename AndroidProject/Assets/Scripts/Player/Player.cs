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
    public StateEventManager<PlayerStates> stateManager = new StateEventManager<PlayerStates>();
    public PlayerState State { private get; set; }
    
    private PlayerMovement _charMovement;
    private PlayerShooting _playerShooting;
    private CinemachineVirtualCamera _vCam;
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(ServiceProvider provider, GameStateManager gameStateManager)
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
        if(stateManager.State == PlayerStates.Ready && _gameStateManager.State == GameState.Countdown)
        {
            OnGameStarted();
        }
        State.Tick();
    }

    public void Kill()
    {
        if (!isLocalPlayer) return;
        stateManager.SetState(PlayerStates.Dead);
        Debug.Log("Player was killed!");
    }

    public void TakeDamage()
    {
        stateManager.SetState(PlayerStates.Dead);
    }

    private void OnGameJoined()
    {
        stateManager.SetState(PlayerStates.Waiting);
    }

    private void OnGameStarted()
    {
        stateManager.SetState(PlayerStates.Playing);
    }
}
