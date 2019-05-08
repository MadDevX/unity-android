using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState: MonoBehaviour
{
    protected Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        Init();
    }
    
    private void OnDestroy()
    {
        Dispose();
    }

    private void AssignState()
    {
        _player.State = this;
        
    }

    protected virtual void Init()
    {
        _player.stateManager.SubscribeToInit(GetState(), AssignState);
    }

    protected virtual void Dispose()
    {
        _player.stateManager.UnsubscribeFromInit(GetState(), AssignState);
    }

    protected abstract PlayerStates GetState();

    public abstract void Tick();
}
