using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            OnPlayerCollision(player);
        }
    }

    protected virtual void OnPlayerCollision(Player player)
    {
        player.Kill();
    }
}
