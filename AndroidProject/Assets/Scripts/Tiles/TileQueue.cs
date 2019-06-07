using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileQueue
{
    public TileBase[] tiles;
    public TilingMode tilingMode;
    private int _currentIndex = 0;

    public TileBase PopTile()
    {
        if(tiles.Length == 0)
        {
            Debug.LogError("TileQueue is empty and PopTile() cannot be invoked.");
            return null;
        }
        switch(tilingMode)
        {
            case TilingMode.Random:
                return tiles[Random.Range(0, tiles.Length)];
            case TilingMode.Sequential:
                int retInd = _currentIndex;
                _currentIndex = (_currentIndex + 1) % tiles.Length;
                return tiles[retInd];
            default:
                return tiles[0];
        }
    }
}
