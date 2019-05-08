using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDead : PlayerState
{
    private Rigidbody2D _rigidbody2D;

    public override void Tick()
    {
    }

    protected override void SetupReferences()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void Init()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Dead;
    }
}
