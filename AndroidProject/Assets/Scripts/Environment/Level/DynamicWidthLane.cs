using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Networking;
using Zenject;
using System;

[CreateAssetMenu(fileName = "NewLane", menuName = "Lanes/Dynamic Width Lane")]
public class DynamicWidthLane : ScriptableObject
{
    [SerializeField]
    private TileQueue _backgroundTiles;
    /// <summary>
    /// Draws lane with specified length and width.
    /// </summary>
    /// <param name="background"></param>
    /// <param name="position"></param>
    /// <param name="laneLength"></param>
    /// <param name="laneWidth"></param>
    /// <returns></returns>
    public int SetupLane(Tilemap background, Vector3Int position, int laneLength, int laneWidth)
    {
        return LaneDrawer.DrawLane(background, _backgroundTiles, position, laneLength, laneWidth);
    }
}
