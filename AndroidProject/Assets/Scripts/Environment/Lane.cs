using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lane : MonoBehaviour
{
    public TileBase tile;

    public void CreateLane(Tilemap tilemap, Vector3Int position, int laneLength)
    {
        for(int i = 0; i < laneLength; i++)
        {
            tilemap.SetTile(position, tile);
            position.y++;
        }
    }
}
