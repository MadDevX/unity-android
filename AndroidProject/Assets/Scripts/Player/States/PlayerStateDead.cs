using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDead : PlayerState
{
    public override void Tick()
    {
        throw new System.NotImplementedException();
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Dead;
    }
}
