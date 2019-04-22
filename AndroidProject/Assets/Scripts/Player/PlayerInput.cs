using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{
    private CharacterMovement _charMovement;
    private CharacterShooting _charShooting;

    void Start()
    {
        _charMovement = gameObject.GetComponent<CharacterMovement>();
        _charShooting = gameObject.GetComponent<CharacterShooting>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        SetMovement();
        SwitchLanes();
    }

    void SetMovement()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _charMovement.IsRunning = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _charMovement.IsRunning = false;
        }
    }

    void SwitchLanes()
    {
        if (Input.GetKeyDown(KeyCode.A)) _charMovement.TurnRight();
        if (Input.GetKeyDown(KeyCode.D)) _charMovement.TurnLeft();
    }
}
