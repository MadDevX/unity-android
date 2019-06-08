using Assets.Scripts.StateMachines;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Player : NetworkBehaviour
{
    /// <summary>
    /// Synchronized variable. Do not change outside the class.
    /// </summary>
    [SyncVar(hook = "OnNicknameChanged")]
    public string Nickname;
    public event Action<string> OnNicknameChangedEvent;
    public EventStateMachine<PlayerStates> stateMachine = new EventStateMachine<PlayerStates>();
    public PlayerState State { private get; set; }
    
    private PlayerMovement _charMovement;
    private PlayerShooting _playerShooting;
    private CinemachineVirtualCamera _vCam;
    private Camera _mainCamera;
    private PrefabManager _prefabManager;
    private GameStateMachine _gameStateMachine;
    private ServiceProvider _serviceProvider;
    private UIManager _uiManager;

    private NickLabel _nickLabel = null;

    [Inject]
    public void Construct(ServiceProvider provider, GameStateMachine gameStateMachine, UIManager uiManager, PrefabManager prefabManager)
    {
        _vCam = provider.vCam;
        _mainCamera = provider.mainCamera;
        _gameStateMachine = gameStateMachine;
        _serviceProvider = provider;
        _uiManager = uiManager;
        _prefabManager = prefabManager;
    }

    private void Awake()
    {
        _charMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    public override void OnStartLocalPlayer()
    {
        _serviceProvider.Player = this;
        CmdSetNickname(_uiManager.joinPanel.nickInputField.text);
        _vCam.Follow = transform;
    }

    private void Start()
    {
        _serviceProvider.allPlayers.Add(this);
        _nickLabel = Instantiate(_prefabManager.nickLabel, _uiManager.gamePanel.transform);
        _nickLabel.Init(this, _mainCamera, _uiManager);
        if (!isLocalPlayer) return;
        OnGameJoined();
    }

    private void OnDestroy()
    {
        _serviceProvider.allPlayers.Remove(this);
        if (_nickLabel != null)
        {
            Destroy(_nickLabel.gameObject);
        }
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

    [Command]
    private void CmdSetNickname(string nick)
    {
        Nickname = nick;
    }

    private void OnNicknameChanged(string nick)
    {
        Nickname = nick;
        OnNicknameChangedEvent?.Invoke(nick);
    }

}
