using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Lane : NetworkBehaviour
{
    public int width;
    public TileQueue backgroundTiles;

    public int SetupLane(Tilemap background, Tilemap interactable, Vector3Int position, int laneLength)
    {
        DrawLane(background, position, laneLength);
        return width;
    }

    private int DrawLane(Tilemap tilemap, Vector3Int position, int laneLength)
    {
        for(int y = 0; y < laneLength; y++)
        {
            var tempPos = position;
            for(int x = 0; x < width; x++)
            {
                tilemap.SetTile(tempPos, backgroundTiles.PopTile());
                tempPos.x++;
            }
            position.y++;
        }

        return width;
    }

}
