using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class LaneDrawer
{
    public static int DrawLane(Tilemap tilemap, TileQueue tileQueue, Vector3Int position, int laneLength, int laneWidth)
    {
        for (int y = 0; y < laneLength; y++)
        {
            var tempPos = position;
            for (int x = 0; x < laneWidth; x++)
            {
                tilemap.SetTile(tempPos, tileQueue.PopTile());
                tempPos.x++;
            }
            position.y++;
        }

        return laneWidth;
    }
}
