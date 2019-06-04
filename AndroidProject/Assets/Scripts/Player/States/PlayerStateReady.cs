using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReady : PlayerState
{
    private Rigidbody2D _rb2D;

    protected override void SetupReferences()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public override void Tick()
    {
        ProcessInput();
        _rb2D.velocity = Vector2.zero;
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
            _player.stateMachine.SetState(PlayerStates.Waiting);
        }
    }
}
