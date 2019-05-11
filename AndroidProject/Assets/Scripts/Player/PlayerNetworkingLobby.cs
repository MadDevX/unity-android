using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerNetworkingLobby : NetworkBehaviour
{
    private Player _player;
    private LobbyManager _lobbyManager;

    [Inject]
    public void Construct(LobbyManager lobbyManager)
    {
        _lobbyManager = lobbyManager;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.stateManager.SubscribeToInit(PlayerStates.Ready, SetReady);        
        _player.stateManager.SubscribeToDispose(PlayerStates.Ready, SetNotReady);
    }

    private void OnDestroy()
    {
        _player.stateManager.UnsubscribeFromInit(PlayerStates.Ready, SetReady);
        _player.stateManager.UnsubscribeFromDispose(PlayerStates.Ready, SetNotReady);

    }

    private void SetReady()
    {
        if(isLocalPlayer)
        {
            if (isServer)
            {
                _lobbyManager.AddReady(this);
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
                _lobbyManager.RemoveReady(this);
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
        _lobbyManager.AddReady(this);
    }

    [Command]
    private void CmdSetNotReady()
    {
        _lobbyManager.RemoveReady(this);
    }
}
