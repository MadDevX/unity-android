using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWaiting : PlayerState
{
    private Rigidbody2D _rb2D;

    private void Awake()
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
            _player.stateMachine.SetState(PlayerStates.Ready);
        }
    }
}
