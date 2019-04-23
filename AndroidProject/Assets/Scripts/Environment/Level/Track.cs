using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Track : NetworkBehaviour
{
    public int LaneCount { get; set; } = 4;
    public Grid tilemapGrid;
    public Tilemap tilemapBase;
    public Tilemap tilemapInteractable;
    public RaceLane raceLanePrefab;
    public Lane borderLeft;
    public Lane borderRight;
    public int laneLength;
    public int minYDistanceBetweenObstacles;
    public int obstacleSpawnOffset;
    public Vector3Int baseVector;
    private List<RaceLane> _lanes = new List<RaceLane>();
    private Vector3Int _offsetVector = new Vector3Int();
    private static Vector3 _tileCorrectionOffset = new Vector3(0.5f, 0.5f, 0.0f);

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InitLevel(false);
    //}

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitLevel(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        InitLevel(false);
    }

    private void InitLevel(bool isServer)
    {
        if (isServer)
        {
            ClearLevel();
        }
        DrawLanes(isServer);
    }

    private void ClearLevel()
    {
        tilemapBase.ClearAllTiles();
        tilemapInteractable.ClearAllTiles();

        foreach(var lane in _lanes)
        {
            Destroy(lane.gameObject);
        }
        _lanes.Clear();

    }

    void DrawLanes(bool isServer)
    {
        _offsetVector.Set(baseVector.x, baseVector.y, baseVector.z);

        _offsetVector.x += borderLeft.SetupLane(tilemapBase, tilemapInteractable, _offsetVector, laneLength);

        List<Vector2Int> blockedYs = new List<Vector2Int>();
        for(int i = 0; i < LaneCount; i++)
        {
            var lane = Instantiate(raceLanePrefab, new Vector3(_offsetVector.x, _offsetVector.y, _offsetVector.z) + _tileCorrectionOffset, 
                                   Quaternion.identity, transform);
            _lanes.Add(lane);
            _offsetVector.x += lane.SetupLane(tilemapBase, tilemapInteractable, _offsetVector, laneLength, blockedYs, minYDistanceBetweenObstacles, obstacleSpawnOffset, isServer);
        }

        _offsetVector.x += borderRight.SetupLane(tilemapBase, tilemapInteractable, _offsetVector, laneLength);
    }
}
