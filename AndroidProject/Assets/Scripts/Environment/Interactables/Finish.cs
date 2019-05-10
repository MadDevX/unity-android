using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Finish : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerRespawn = collision.GetComponent<PlayerRespawn>();
        if(playerRespawn != null)
        {
            playerRespawn.Respawn();
        }
    }
}
