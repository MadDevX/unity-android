using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour
{
    public float movementSpeed = 2.0f;
    public float runMult = 2.0f;

    public bool IsRunning { get; set; } = false;
    private int _lane;

    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private Collider2D _leftLane;
    [SerializeField]
    private Collider2D _rightLane;

    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
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
        Debug.Log("weszlo");
        _lane += direction;
        _rigidbody2D.MovePosition(_rigidbody2D.position + new Vector2(direction, 0));
        return true;
    }
}
