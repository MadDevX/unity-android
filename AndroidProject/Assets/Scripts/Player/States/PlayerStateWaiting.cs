using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWaiting : PlayerState
{
    private PlayerMovement _playerMovement;

    protected override void SetupReferences()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public override void Tick()
    {
        ProcessInput();
        _playerMovement.ResetVelocity();
    }

    protected override void Init()
    {
        Debug.Log("Player started waiting.");
    }

    protected override void Dispose()
    {
        Debug.Log("Player stopped waiting.");
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Waiting;
    }

    private void ProcessInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            _player.SetPlayerState(PlayerStates.Ready);
        }
    }
}
