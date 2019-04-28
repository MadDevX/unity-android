using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSettings : MonoBehaviour
{
    public RaceLane raceLanePrefab;
    public Lane borderLane;

    public int LaneCount { get; set; } = 4;
    public int laneLength;
    public int minYDistanceBetweenObstacles;
    public int obstacleSpawnOffset;
    public Vector3Int baseVector;
    public Vector3 tileCorrectionOffset = new Vector3(0.5f, 0.5f, 0.0f);
}
