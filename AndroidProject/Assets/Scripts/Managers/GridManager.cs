using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemapBase;
    public Tilemap tilemapInteractable;

    public void ClearTilemaps()
    {
        tilemapBase.ClearAllTiles();
        tilemapInteractable.ClearAllTiles();
    }
}
