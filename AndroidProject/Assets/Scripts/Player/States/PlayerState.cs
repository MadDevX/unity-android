using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState: MonoBehaviour
{
    protected Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        SetupReferences();
        _player.stateManager.SubscribeToInit(GetState(), AssignState);
        _player.stateManager.SubscribeToInit(GetState(), Init);
        _player.stateManager.SubscribeToDispose(GetState(), Dispose);
    }
    
    private void OnDestroy()
    {
        _player.stateManager.UnsubscribeFromInit(GetState(), AssignState);
        _player.stateManager.UnsubscribeFromInit(GetState(), Init);
        _player.stateManager.UnsubscribeFromDispose(GetState(), Dispose);
    }

    private void AssignState()
    {
        _player.State = this;
    }

    /// <summary>
    /// Used to set up references to other components from game object.
    /// </summary>
    protected virtual void SetupReferences()
    {
    }

    /// <summary>
    /// Invoked if player state changes to this state.
    /// </summary>
    protected virtual void Init()
    {
    }

    /// <summary>
    /// Invoked if player state changes from this state to another state.
    /// </summary>
    protected virtual void Dispose()
    {
    }

    protected abstract PlayerStates GetState();

    public abstract void Tick();
}
