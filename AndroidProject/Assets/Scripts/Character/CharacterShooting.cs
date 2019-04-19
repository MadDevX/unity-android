using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterShooting : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 offset;
    public float bulletSpeed;

    [Command]
    public void CmdFire()
    {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.rotation * offset, transform.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

}
