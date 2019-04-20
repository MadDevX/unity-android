using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Track : MonoBehaviour
{
    public Grid tilemapGrid;
    public Tilemap tilemapBase;
    public Tilemap tilemapInteractable;
    public List<Lane> lanes;
    public Lane borderLeft;
    public Lane borderRight;
    public int laneLength;
    public Vector3Int baseVector;
    private Vector3Int offsetVector = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        tilemapBase.ClearAllTiles();

        DrawLanes();
    }

    void DrawLanes()
    {
        offsetVector.Set(baseVector.x, baseVector.y, baseVector.z);

        offsetVector.x += borderLeft.DrawLane(tilemapBase, offsetVector, laneLength);

        foreach (var lane in lanes)
        {
            offsetVector.x += lane.DrawLane(tilemapBase, offsetVector, laneLength);
        }

        offsetVector.x += borderRight.DrawLane(tilemapBase, offsetVector, laneLength);
    }
}
