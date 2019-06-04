using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerShooting : NetworkBehaviour
{

    private PrefabManager _prefabManager;
    private PlayerSettings _playerSettings;
    private PlayerMovement _playerMovement;

    [Inject]
    public void Construct(PrefabManager prefabManager,PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
        _prefabManager = prefabManager;
    }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }


    [Command]
    public void CmdFire()
    {

            
        var bullet = Instantiate(_prefabManager.bullet, transform.position + _playerSettings.offset, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * _playerSettings.bulletSpeed + new Vector3(0.0f, _playerMovement.GetVelocityVector().y, 0.0f);

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 1.0f);
    }



}
