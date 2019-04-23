using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour
{
    public float movementSpeed = 2.0f;
    public float runMult = 2.0f;
    public float lerpFactor = 0.1f;

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
    private int _xPos;
    private float _xTileOffset = 0.5f;

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
        _xPos = (int)_rigidbody2D.position.x;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        Vector2 movementVector = Vector2.zero;
        movementVector += MovementVector();
        movementVector += CorrectPositionVector();
        ApplyMovementVector(movementVector);
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

    private Vector2 MovementVector()
    {
        var currentVelocity = Vector2.up * movementSpeed;
        if (IsRunning) currentVelocity *= runMult;
        return currentVelocity * Time.fixedDeltaTime;
    }

    private Vector2 CorrectPositionVector()
    {
        float interpolateX = Mathf.Lerp(0.0f, _xPos + _xTileOffset - _rigidbody2D.position.x, lerpFactor);
        return Vector2.right * interpolateX;
    }

    private void ApplyMovementVector(Vector2 vec)
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + vec);
    }

    private bool SwitchLanes(int direction)
    {
        _xPos += direction;
        return true;
    }
}
