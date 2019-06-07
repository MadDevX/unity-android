using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrefabManager : MonoBehaviour
{
    public TileBase finishTile;
    public TileQueue backgroundTiles;
    public List<TileEntry> obstacles;
    public StaticWidthLane borderLane;
    public DynamicWidthLane trackLane;
    public GameObject spawnPoint;
    public GameObject bullet;
    public ScoreListEntry scoreListEntry;

    [System.Serializable]
    public class TileEntry : IRandomizable
    {
        public TileBase tile;
        [Range(0.0f, 1.0f)]
        public float probability;

        public float GetProbability()
        {
            return probability;
        }
    }
}
