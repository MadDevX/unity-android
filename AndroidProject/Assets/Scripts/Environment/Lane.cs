using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lane : MonoBehaviour
{
    public int width;
    public TileQueue tileQueue;

    public int DrawLane(Tilemap tilemap, Vector3Int position, int laneLength)
    {
        for(int y = 0; y < laneLength; y++)
        {
            var tempPos = position;
            for(int x = 0; x < width; x++)
            {
                tilemap.SetTile(tempPos, tileQueue.PopTile());
                tempPos.x++;
            }
            position.y++;
        }

        return width;
    }

}
