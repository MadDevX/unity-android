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
        Shoot();
    }

    void SetMovement()
    {
        _charMovement.Horizontal = Input.GetAxis("Horizontal");
        _charMovement.Vertical = Input.GetAxis("Vertical");
    }

    void Shoot()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            _charShooting.CmdFire();
        }
    }
}
