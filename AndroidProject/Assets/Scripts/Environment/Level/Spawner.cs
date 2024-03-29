﻿using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Spawner : MonoBehaviour
{
    private ConnectionStateMachine _connManager;
    private EnvironmentSettings _envSettings;
    private PrefabManager _prefabManager;
    private GridManager _gridManager;
    private Track _track;
    private List<GameObject> _spawnPoints = new List<GameObject>();

    [Inject]
    public void Construct(EnvironmentSettings envSettings, PrefabManager prefabManager, GridManager gridManager, Track track, ConnectionStateMachine connManager)
    {
        _envSettings = envSettings;
        _prefabManager = prefabManager;
        _gridManager = gridManager;
        _track = track;
        _connManager = connManager;
    }

    private void Awake()
    {
        _track.OnMapGenerated += GenerateInteractables;
        _track.OnMapCleared += ClearInteractables;
    }

    private void OnDestroy()
    {
        _track.OnMapGenerated -= GenerateInteractables;
        _track.OnMapCleared -= ClearInteractables;
    }

    private void SubscribeMethods()
    {
    }

    private void UnsubscribeMethods()
    {
    }

    private void GenerateInteractables(int boundsMin, int boundsMax)
    {
        if (_connManager.State == ConnectionState.Host)
        {
            GenerateObstacles(boundsMin, boundsMax);
            GenerateFinish(boundsMin, boundsMax);
            GenerateSpawnPoints(boundsMin, boundsMax);
        }
    }

    private void GenerateObstacles(int boundsMin, int boundsMax)
    {
        int lastX = boundsMin - 2;
        int lastY = _envSettings.obstacleSpawnOffset;
        for (int y = 0; y < _envSettings.laneLength - 1; y++)
        {
            if (y < lastY + _envSettings.minYDistanceBetweenObstacles) continue;

            lastX = boundsMin - 2;
            for (int x = boundsMin; x <= boundsMax; x++)
            {
                if (lastX + 1 == x) continue;
                
                if(Random.Range(0.0f, 1.0f) < _envSettings.obstacleSpawnChance)
                {
                    var position = new Vector3Int(x, y, 0);
                    _gridManager.tilemapInteractable.SetTile(position, RandomizedSet<PrefabManager.TileEntry>.Generate(_prefabManager.obstacles).tile);
                    lastX = x;
                    lastY = y;
                }
            }
        }
    }

    private void GenerateSpawnPoints(int boundsMin, int boundsMax)
    {
        for (int x = boundsMin; x <= boundsMax; x++)
        {
            var position = new Vector3Int(x, 0, 0);
            var spawnPoint = Instantiate(_prefabManager.spawnPoint, position + _envSettings.tileCorrectionOffset, Quaternion.identity);
            _spawnPoints.Add(spawnPoint);
            NetworkServer.Spawn(spawnPoint);
        }
    }

    private void GenerateFinish(int boundsMin, int boundsMax)
    {
        for (int x = boundsMin; x <= boundsMax; x++)
        {
            var position = new Vector3Int(x, _envSettings.laneLength - 1, 0);
            _gridManager.tilemapInteractable.SetTile(position, _prefabManager.finishTile);
        }
    }

    private void ClearInteractables()
    {
        if (_connManager.State == ConnectionState.Host)
        {
            _gridManager.tilemapInteractable.ClearAllTiles();
            ClearSpawnPoints();
        }
    }

    private void ClearSpawnPoints()
    {
        foreach(var spawnPoint in _spawnPoints)
        {
            if(spawnPoint != null)
            {
                Destroy(spawnPoint);
            }
        }
        _spawnPoints.Clear();
    }
}
