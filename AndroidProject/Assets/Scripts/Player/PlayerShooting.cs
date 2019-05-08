using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerShooting : NetworkBehaviour
{

    private PrefabManager _prefabManager;
    private PlayerSettings _playerSettings;
    private Rigidbody2D _rigidbody2D;

    [Inject]
    public void Construct(PrefabManager prefabManager,PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
        _prefabManager = prefabManager;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    [Command]
    public void CmdFire()
    {

            
        var bullet = Instantiate(_prefabManager.bullet, transform.position + transform.rotation * _playerSettings.offset, transform.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * _playerSettings.bulletSpeed + new Vector3(0.0f,_rigidbody2D.velocity.y,0.0f);

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 1.0f);
    }



}
