using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDead : PlayerState
{
    private PlayerMovement _playerMovement;
    private Animator _anim;

    public override void Tick()
    {
        _playerMovement.ResetVelocityVertical();
    }

    protected override void SetupReferences()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _anim = GetComponent<Animator>();
    }

    protected override void Init()
    {
        _playerMovement.ResetVelocity();
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
