using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWaiting : PlayerState
{
    public override void Tick()
    {

    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Waiting;
    }
}
