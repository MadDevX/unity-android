using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RaceLane : Lane
{
    [Range(0.0f, 1.0f)]
    public float spawnChance;
    public List<TileEntry> interactableTiles;

    /// <summary>
    /// Draws background tiles and generates obstacles
    /// </summary>
    /// <param name="background">Tilemap on which background tiles will be drawn.</param>
    /// <param name="interactable">Tilemap on which interactable tiles will be drawn.</param>
    /// <param name="position">Position of the first tile that will be drawn in a lane.</param>
    /// <param name="laneLength">Length of the drawn lane.</param>
    /// <param name="blockedTilesY">List of y coordinates telling which y's are already blocked by obstacle from another lane.</param>
    /// <param name="minYDistanceBetweenObstacles">Minimal y distance between spawned obstacles on lanes.</param>
    /// <returns></returns>
    public int SetupLane(Tilemap background, Tilemap interactable, Vector3Int position, int laneLength, List<Vector2Int> blockedTilesY,
                         int minYDistanceBetweenObstacles, int obstacleSpawnOffset)
    {
        GenerateInteractables(interactable, position, laneLength, blockedTilesY, minYDistanceBetweenObstacles, obstacleSpawnOffset);
        return base.SetupLane(background, interactable, position, laneLength);
    }

    private List<Vector2Int> GenerateInteractables(Tilemap colliderTilemap, Vector3Int position, int laneLength, List<Vector2Int> blockedTilesY, 
                                            int minYDistanceBetweenObstacles, int obstacleSpawnOffset, int initialOffset = 0)
    {
        int currentOffset = 0;
        for (int y = 0; y < laneLength; y++)
        {
            var tempPos = position;
            for (int x = 0; x < width; x++)
            {
                if (CanSpawn((Vector2Int)tempPos, blockedTilesY, minYDistanceBetweenObstacles) && Random.value < spawnChance && currentOffset >= obstacleSpawnOffset)
                {
                    colliderTilemap.SetTile(tempPos, RandomizedSet<TileEntry>.Generate(interactableTiles).tile);
                    blockedTilesY.Add((Vector2Int)tempPos);
                    currentOffset = 0;
                }
                tempPos.x++;
            }
            position.y++;
            currentOffset++;
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
