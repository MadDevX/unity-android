using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerMovement : NetworkBehaviour
{
    public bool IsRunning {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            _anim.speed = _isRunning ? _settings.runMult : 1.0f;
        }
    }

    private bool _isRunning = false;
    private int _xTilePos;
    private float _xTileOffset = 0.5f;

    [SerializeField]
    private Collider2D _leftLane;
    [SerializeField]
    private Collider2D _rightLane;

    private Rigidbody2D _rigidbody2D;
    private Animator _anim;

    private PlayerSettings _settings;

    [Inject]
    public void Construct(PlayerSettings playerSettings)
    {
        _settings = playerSettings;
    }


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _xTilePos = (int)_rigidbody2D.position.x;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        Vector2 movementVector = Vector2.zero;
        Accelerate();
        CorrectPosition();

    }

    private void Accelerate()
    {
        if (IsRunning)
        {
            _rigidbody2D.velocity += new Vector2(_rigidbody2D.velocity.x, _settings.accelerationRate);
        }
    }

    public void SetLane(int lane)
    {
        _xTilePos = lane;
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
        var currentVelocity = Vector2.up * _settings.movementSpeed;
        if (IsRunning) currentVelocity *= _settings.runMult;
        return currentVelocity * Time.fixedDeltaTime;
    }

    private void CorrectPosition()
    {
        float interpolateX = Mathf.Lerp(0.0f, _xTilePos + _xTileOffset - _rigidbody2D.position.x, _settings.lerpFactor);
        _rigidbody2D.velocity = new Vector2(interpolateX/Time.fixedDeltaTime, _rigidbody2D.velocity.y);
    }

    private void ApplyMovementVector(Vector2 vec)
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + vec);
    }

    private bool SwitchLanes(int direction)
    {
        _xTilePos += direction;
        return true;
    }
}
