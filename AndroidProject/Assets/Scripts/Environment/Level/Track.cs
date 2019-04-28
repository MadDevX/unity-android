using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using Zenject;

public class Track : NetworkBehaviour
{
    private List<RaceLane> _lanes = new List<RaceLane>();
    private Vector3Int _offsetVector = new Vector3Int();

    private GridManager _gridManager;
    private EnvironmentSettings _envSettings;

    [Inject]
    public void Construct(GridManager gridManager, EnvironmentSettings envSettings)
    {
        _gridManager = gridManager;
        _envSettings = envSettings;
    }

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

    public void AddRaceLane(RaceLane lane)
    {
        _lanes.Add(lane);
    }

    private void RefreshRaceLanes()
    {
        ClearRaceLanes();
        Vector3Int baseVec = _envSettings.baseVector;
        baseVec.x += _envSettings.borderLane.width;
        InitRaceLanes(baseVec, _envSettings.LaneCount);
    }

    private void InitLevel(bool isServer)
    {
        if (isServer)
        {
            _gridManager.ClearTilemaps();
            RefreshRaceLanes();
        }
        DrawLanes(isServer);
    }

    private void ClearRaceLanes()
    {
        foreach (var lane in _lanes)
        {
            if (lane != null && lane.gameObject != null)
            {
                Destroy(lane.gameObject);
            }
        }

        _lanes.Clear();
    }

    private RaceLane CreateRaceLane(Vector3Int currentOffset)
    {
        var lane = Instantiate(_envSettings.raceLanePrefab, new Vector3(currentOffset.x, currentOffset.y, currentOffset.z) + _envSettings.tileCorrectionOffset,
                       Quaternion.identity);
        lane.SetOffset(currentOffset);
        NetworkServer.Spawn(lane.gameObject);
        return lane;
    }

    void InitRaceLanes(Vector3Int currentOffset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var lane = CreateRaceLane(currentOffset);
            currentOffset.x += lane.width;
        }
    }

    void DrawLanes(bool isServer)
    {
        _offsetVector = _envSettings.baseVector;

        _offsetVector.x += _envSettings.borderLane.SetupLane(_gridManager.tilemapBase, _offsetVector, _envSettings.laneLength);

        List<Vector2Int> blockedYs = new List<Vector2Int>();

        for(int i = 0; i < _lanes.Count; i++)
        {
            _offsetVector.x += _lanes[i].SetupLane(_gridManager.tilemapBase, _lanes[i].GetOffset(), _envSettings.laneLength);
            if (isServer) _lanes[i].SetupInteractables(_gridManager.tilemapInteractable, blockedYs);
        }

        _offsetVector.x += _envSettings.borderLane.SetupLane(_gridManager.tilemapBase, _offsetVector, _envSettings.laneLength);
    }
}
