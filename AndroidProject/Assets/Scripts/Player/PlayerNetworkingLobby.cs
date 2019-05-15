using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerNetworkingLobby : NetworkBehaviour
{
    [SerializeField]
    private GameObject _readyIndicator;
    private Player _player;
    private LobbyManager _lobbyManager;
    private Vector3 _lobbySpawnPoint;
    private PlayerMovement _playerMovement;

    [Inject]
    public void Construct(LobbyManager lobbyManager, ServiceProvider provider)
    {
        _lobbyManager = lobbyManager;
        _lobbySpawnPoint = provider.lobbySpawnPoint.position;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerMovement = GetComponent<PlayerMovement>();
        _player.stateMachine.SubscribeToInit(PlayerStates.Ready, SetReady);        
        _player.stateMachine.SubscribeToDispose(PlayerStates.Ready, SetNotReady);
    }

    private void OnDestroy()
    {
        _player.stateMachine.UnsubscribeFromInit(PlayerStates.Ready, SetReady);
        _player.stateMachine.UnsubscribeFromDispose(PlayerStates.Ready, SetNotReady);

    }

    public override void OnStartLocalPlayer()
    {
        _playerMovement.SetPosition(_lobbySpawnPoint);
    }

    private void SetReady()
    {
        if(isLocalPlayer)
        {
            if (isServer)
            {
                SetReadyServer();
            }
            else
            {
                CmdSetReady();
            }
        }
    }

    private void SetNotReady()
    {
        if(isLocalPlayer)
        {
            if (isServer)
            {
                SetNotReadyServer();
            }
            else
            {
                CmdSetNotReady();
            }
        }
    }

    [Command]
    private void CmdSetReady()
    {
        SetReadyServer();
    }

    [Command]
    private void CmdSetNotReady()
    {
        SetNotReadyServer();
    }

    private void SetReadyServer()
    {
        _lobbyManager.AddReady(this);
        RpcSetIndicator(true);
    }

    private void SetNotReadyServer()
    {
        _lobbyManager.RemoveReady(this);
        RpcSetIndicator(false);
    }


    [ClientRpc]
    private void RpcSetIndicator(bool active)
    {
        _readyIndicator.SetActive(active);
    }
}
