using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDead : PlayerState
{
    private Rigidbody2D _rigidbody2D;
    private Animator _anim;

    public override void Tick()
    {
    }

    protected override void SetupReferences()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    protected override void Init()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _anim.SetBool("IsDead", true);
    }

    protected override void Dispose()
    {
        _anim.SetBool("IsDead", false);
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Dead;
    }
}
