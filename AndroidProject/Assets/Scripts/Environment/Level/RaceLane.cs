using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Networking;
using Zenject;

public class RaceLane : Lane
{
    [Range(0.0f, 1.0f)]
    public float spawnChance;
    public List<TileEntry> interactables;
    public TileBase finishTile;

    [SyncVar]
    public int offsetX;
    [SyncVar]
    public int offsetY;
    [SyncVar]
    public int offsetZ;

    private EnvironmentSettings _envSettings;

    [Inject]
    public void Construct(Track environment, EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        environment.AddRaceLane(this);
    }

    public Vector3Int GetOffset()
    {
        return new Vector3Int(offsetX, offsetY, offsetZ);
    }

    public void SetOffset(Vector3Int vector)
    {
        offsetX = vector.x;
        offsetY = vector.y;
        offsetZ = vector.z;
    }

    public void SetupInteractables(Tilemap interactable, List<Vector2Int> blockedTilesY)
    {
        GenerateInteractables(interactable, GetOffset(), _envSettings.laneLength, blockedTilesY, _envSettings.minYDistanceBetweenObstacles, _envSettings.obstacleSpawnOffset);
    }

    private List<Vector2Int> GenerateInteractables(Tilemap colliderTilemap, Vector3Int position, int laneLength, List<Vector2Int> blockedTilesY, 
                                            int minYDistanceBetweenObstacles, int obstacleSpawnOffset, int initialOffset = 0)
    {
        int currentOffset = 0;
        int y;
        Vector3Int tempPos;
        //Generating obstacles
        for (y = 0; y < laneLength-1; y++)
        {
            tempPos = position;
            for (int x = 0; x < width; x++)
            {
                if (CanSpawn((Vector2Int)tempPos, blockedTilesY, minYDistanceBetweenObstacles) && Random.value < spawnChance && currentOffset >= obstacleSpawnOffset)
                {
                    colliderTilemap.SetTile(tempPos, RandomizedSet<TileEntry>.Generate(interactables).tile);
                    blockedTilesY.Add((Vector2Int)tempPos);
                    currentOffset = 0;
                }
                tempPos.x++;
            }
            position.y++;
            currentOffset++;
        }
        //Generating finish line
        tempPos = position;
        for(int x = 0; x < width; x++)
        {
            colliderTilemap.SetTile(tempPos, finishTile);
            tempPos.x++;
        }
        return blockedTilesY;
    }

    private bool CanSpawn(Vector2Int position, List<Vector2Int> blockedTilesY, int minYDistance)
    {
        if (blockedTilesY == null) return true;
        foreach(var vector in blockedTilesY)
        {
            if (Mathf.Abs(position.x - vector.x) < 2 && //This only checks adjacent lanes - not adjacent columns can generate obstacles on the same height.
                Mathf.Abs(position.y - vector.y) < minYDistance)
                return false;
        }
        return true;
    }

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
