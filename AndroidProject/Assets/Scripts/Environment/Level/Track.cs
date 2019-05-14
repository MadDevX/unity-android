using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Track : MonoBehaviour
{
    /// <summary>
    /// Contains x coordinate of first playable lane.
    /// </summary>
    public int GameAreaBoundsMin { get; private set; }
    /// <summary>
    /// Contains x coordinate of last playable lane.
    /// </summary>
    public int GameAreaBoundsMax { get; private set; }

    public event Action<int, int> OnMapGenerated;
    public event Action OnMapCleared;

    private Vector3Int _offsetVector = new Vector3Int();
    private GridManager _gridManager;
    private EnvironmentSettings _envSettings;
    private GameStateMachine _gameStateManager;
    private PrefabManager _prefabManager;
    [Inject]
    public void Construct(GridManager gridManager, 
                          EnvironmentSettings envSettings, 
                          GameStateMachine gameStateManager,
                          PrefabManager prefabManager)
    {
        _gridManager = gridManager;
        _envSettings = envSettings;
        _gameStateManager = gameStateManager;
        _prefabManager = prefabManager;
    }

    private void Awake()
    {
        _gameStateManager.SubscribeToInit(GameState.Countdown, DrawTrack);
        _gameStateManager.SubscribeToDispose(GameState.Finished, ClearTrack);
    }

    private void OnDestroy()
    {
        _gameStateManager.UnsubscribeFromInit(GameState.Countdown, DrawTrack);
        _gameStateManager.UnsubscribeFromDispose(GameState.Finished, ClearTrack);
    }

    void DrawTrack(GameStateEventArgs e)
    {
        _offsetVector = _envSettings.baseVector;

        _offsetVector.x += _prefabManager.borderLane.SetupLane(_gridManager.tilemapBase, _offsetVector, _envSettings.laneLength);
        
        GameAreaBoundsMin = _offsetVector.x;
        _offsetVector.x += _prefabManager.trackLane.SetupLane(_gridManager.tilemapBase, _offsetVector, _envSettings.laneLength, e.playerCount + _envSettings.extraLanes);
        GameAreaBoundsMax = _offsetVector.x - 1;

        _offsetVector.x += _prefabManager.borderLane.SetupLane(_gridManager.tilemapBase, _offsetVector, _envSettings.laneLength);

        OnMapGenerated?.Invoke(GameAreaBoundsMin, GameAreaBoundsMax);
    }

    void ClearTrack(GameStateEventArgs e)
    {
        _gridManager.tilemapBase.ClearAllTiles();
        OnMapCleared?.Invoke();
    }
}
