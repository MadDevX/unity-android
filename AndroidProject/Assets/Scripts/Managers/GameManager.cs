using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class GameManager : NetworkBehaviour
{
    private GameStateManager _manager;

    [Inject]
    public void Construct(GameStateManager manager)
    {
        _manager = manager;
    }

    private void Awake()
    {
        Action act0 = () => { Debug.Log("Started init"); };
        Action act1 = () => { Debug.Log("Started dispose"); };
        Action act2 = () => { Debug.Log("Menu init"); };
        Action act3 = () => { Debug.Log("Menu dispose"); };
        _manager.SubscribeToInit(GameState.Started, act0);
        _manager.SubscribeToDispose(GameState.Started, act1);
        _manager.SubscribeToInit(GameState.Menu, act2);
        _manager.SubscribeToDispose(GameState.Menu, act3);
        _manager.SetState(GameState.Started);
        _manager.SetState(GameState.Menu);
        _manager.SetState(GameState.Started);
        _manager.UnsubscribeFromDispose(GameState.Started, act1);
        _manager.SetState(GameState.Finished);
    }

    public void StartGame()
    {

    }

    public void PauseGame()
    {

    }


}
