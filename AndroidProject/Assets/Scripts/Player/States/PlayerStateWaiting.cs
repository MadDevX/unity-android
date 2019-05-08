using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWaiting : PlayerState
{
    public override void Tick()
    {
        ProcessInput();
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
        if(Input.GetButton("Fire1"))
        {
            _player.stateManager.SetState(PlayerStates.Ready);
        }
    }
}
