using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileQueue
{
    public TileBase[] tiles;
    public TilingMode tilingMode;
    private int currentIndex = 0;

    public TileBase PopTile()
    {
        switch(tilingMode)
        {
            case TilingMode.Random:
                return tiles[Random.Range(0, tiles.Length)];
            case TilingMode.Sequential:
                int retInd = currentIndex;
                currentIndex = (currentIndex + 1) % tiles.Length;
                return tiles[retInd];
            default:
                return tiles[0];
        }
    }
}
