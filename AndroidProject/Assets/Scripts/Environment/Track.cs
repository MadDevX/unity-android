using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Track : MonoBehaviour
{
    public Grid tilemapGrid;
    public Tilemap tilemapBase;
    public Tilemap tilemapInteractable;
    public List<RaceLane> lanes;
    public Lane borderLeft;
    public Lane borderRight;
    public int laneLength;
    public int minYDistanceBetweenObstacles;
    public int obstacleSpawnOffset;
    public Vector3Int baseVector;
    private Vector3Int offsetVector = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
    }

    private void InitLevel()
    {
        tilemapBase.ClearAllTiles();
        tilemapInteractable.ClearAllTiles();

        DrawLanes();
    }

    void DrawLanes()
    {
        offsetVector.Set(baseVector.x, baseVector.y, baseVector.z);

        offsetVector.x += borderLeft.SetupLane(tilemapBase, tilemapInteractable, offsetVector, laneLength);

        List<Vector2Int> blockedYs = new List<Vector2Int>();
        foreach (var lane in lanes)
        {
            offsetVector.x += lane.SetupLane(tilemapBase, tilemapInteractable, offsetVector, laneLength, blockedYs, minYDistanceBetweenObstacles, obstacleSpawnOffset);
        }

        offsetVector.x += borderRight.SetupLane(tilemapBase, tilemapInteractable, offsetVector, laneLength);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InitLevel();
        }
        if(Input.GetMouseButtonDown(1))
        {
            tilemapInteractable.ClearAllTiles();
        }
    }
}
