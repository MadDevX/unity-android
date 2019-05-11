using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReady : PlayerState
{
    public override void Tick()
    {
        ProcessInput();
    }

    protected override void Init()
    {
        Debug.Log("Player is ready.");
    }

    protected override void Dispose()
    {
        Debug.Log("Player is no longer ready.");
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Ready;
    }

    private void ProcessInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            _player.stateManager.SetState(PlayerStates.Waiting);
        }
    }
}
