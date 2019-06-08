using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReady : PlayerState
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

    protected override PlayerStates GetState()
    {
        return PlayerStates.Ready;
    }

    private void ProcessInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            _player.SetPlayerState(PlayerStates.Waiting);
        }
    }
}
