using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObstacle : Obstacle
{
    protected override void OnPlayerCollision(Player player)
    {
        player.PaintRandom();
    }
}
