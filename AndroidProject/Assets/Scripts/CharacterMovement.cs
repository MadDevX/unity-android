using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 2.0f;

    public float Horizontal { get; set; } = 0.0f;
    public float Vertical { get; set; } = 0.0f;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var currentVelocity = new Vector2(Horizontal, Vertical).normalized * movementSpeed;
        _rigidbody2D.MovePosition(_rigidbody2D.position + currentVelocity * Time.fixedDeltaTime);
        //transform.position += currentVelocity * Time.deltaTime;
    }
}
