using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterMovement _charMovement;

    void Start()
    {
        _charMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        _charMovement.Horizontal = Input.GetAxis("Horizontal");
        _charMovement.Vertical = Input.GetAxis("Vertical");
    }
}
