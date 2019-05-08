using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerShooting : NetworkBehaviour
{

    private PrefabManager _prefabManager;
    private PlayerSettings _playerSettings;

    [Inject]
    public void Construct(PrefabManager prefabManager,PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
        _prefabManager = prefabManager;
    }

    [Command]
    public void CmdFire()
    {

            
        var bullet = Instantiate(_prefabManager.bullet, transform.position + transform.rotation * _playerSettings.offset, transform.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * _playerSettings.bulletSpeed;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }



}
