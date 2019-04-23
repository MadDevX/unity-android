using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour
{
    public float movementSpeed = 2.0f;
    public float runMult = 2.0f;

    private bool _isRunning = false;
    public bool IsRunning {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            _anim.speed = _isRunning ? runMult : 1.0f;
        }
    }
    private int _lane;

    [SerializeField]
    private Collider2D _leftLane;
    [SerializeField]
    private Collider2D _rightLane;

    private Rigidbody2D _rigidbody2D;
    private Animator _anim;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        Move();
    }

    public bool TurnLeft()
    {
        if (_leftLane.IsTouchingLayers()) return false;
        return SwitchLanes(-1);
    }

    public bool TurnRight()
    {
        if (_rightLane.IsTouchingLayers()) return false;
        return SwitchLanes(1);
    }

    private void Move()
    {
        var currentVelocity = Vector2.up * movementSpeed;
        if (IsRunning) currentVelocity *= runMult;
        _rigidbody2D.MovePosition(_rigidbody2D.position + currentVelocity * Time.fixedDeltaTime);
    }

    private bool SwitchLanes(int direction)
    {
        _lane += direction;
        _rigidbody2D.position = (_rigidbody2D.position + new Vector2(direction, 0));
        return true;
    }
}
