using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour
{
    public float movementSpeed = 2.0f;

    public float Horizontal { get; set; } = 0.0f;
    public float Vertical { get; set; } = 0.0f;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        Move();
    }

    private void Move()
    {
        var currentVelocity = new Vector2(Horizontal, Vertical).normalized * movementSpeed;
        _rigidbody2D.MovePosition(_rigidbody2D.position + currentVelocity * Time.fixedDeltaTime);
    }
}
