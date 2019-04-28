using System;
using System.Collections.Generic;


//Without arguments
public class StateEventManager<EnumType> where EnumType : struct, IConvertible
{
    public EnumType State { get; private set; }

    private Dictionary<EnumType, Action> _onStateInitialized = new Dictionary<EnumType, Action>();
    private Dictionary<EnumType, Action> _onStateDisposed = new Dictionary<EnumType, Action>();

    public void SetState(EnumType state)
    {
        GetOnDisposeEvent(State)?.Invoke();
        State = state;
        GetOnInitEvent(state)?.Invoke();
    }

    public StateEventManager()
    {
        InitDictionaries();
    }

    public void SubscribeToInit(EnumType state, Action method)
    {
        var evt = GetOnInitEvent(state);
        evt += method;
        SetOnInitEvent(state, evt);
    }

    public void UnsubscribeFromInit(EnumType state, Action method)
    {
        var evt = GetOnInitEvent(state);
        evt -= method;
        SetOnInitEvent(state, evt);
    }

    public void SubscribeToDispose(EnumType state, Action method)
    {
        var evt = GetOnDisposeEvent(state);
        evt += method;
        SetOnDisposeEvent(state, evt);
    }

    public void UnsubscribeFromDispose(EnumType state, Action method)
    {
        var evt = GetOnDisposeEvent(state);
        evt -= method;
        SetOnDisposeEvent(state, evt);
    }

    private void InitDictionaries()
    {
        foreach (EnumType state in Enum.GetValues(typeof(EnumType)))
        {
            _onStateInitialized.Add(state, null);
            _onStateDisposed.Add(state, null);
        }
    }

    private Action GetOnInitEvent(EnumType state)
    {
        return _onStateInitialized[state];
    }

    private Action GetOnDisposeEvent(EnumType state)
    {
        return _onStateDisposed[state];
    }

    private void SetOnInitEvent(EnumType state, Action evt)
    {
        _onStateInitialized[state] = evt;
    }

    private void SetOnDisposeEvent(EnumType state, Action evt)
    {
        _onStateDisposed[state] = evt;
    }
}

//With arguments
public class StateEventManager<EnumType, ActionArgs> where EnumType : struct, IConvertible
{
    public EnumType State { get; private set; }

    private Dictionary<EnumType, Action<ActionArgs>> _onStateInitialized = new Dictionary<EnumType, Action<ActionArgs>>();
    private Dictionary<EnumType, Action<ActionArgs>> _onStateDisposed = new Dictionary<EnumType, Action<ActionArgs>>();
    
    public void SetState(EnumType state, ActionArgs e)
    {
        GetOnDisposeEvent(State)?.Invoke(e);
        State = state;
        GetOnInitEvent(state)?.Invoke(e);
    }

    public StateEventManager()
    {
        InitDictionaries();
    }

    public void SubscribeToInit(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnInitEvent(state);
        evt += method;
        SetOnInitEvent(state, evt);
    }

    public void UnsubscribeFromInit(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnInitEvent(state);
        evt -= method;
        SetOnInitEvent(state, evt);
    }

    public void SubscribeToDispose(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnDisposeEvent(state);
        evt += method;
        SetOnDisposeEvent(state, evt);
    }

    public void UnsubscribeFromDispose(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnDisposeEvent(state);
        evt -= method;
        SetOnDisposeEvent(state, evt);
    }

    private void InitDictionaries()
    {
        foreach (EnumType state in Enum.GetValues(typeof(EnumType)))
        {
            _onStateInitialized.Add(state, null);
            _onStateDisposed.Add(state, null);
        }
    }

    private Action<ActionArgs> GetOnInitEvent(EnumType state)
    {
        return _onStateInitialized[state];
    }

    private Action<ActionArgs> GetOnDisposeEvent(EnumType state)
    {
        return _onStateDisposed[state];
    }

    private void SetOnInitEvent(EnumType state, Action<ActionArgs> evt)
    {
        _onStateInitialized[state] = evt;
    }

    private void SetOnDisposeEvent(EnumType state, Action<ActionArgs> evt)
    {
        _onStateDisposed[state] = evt;
    }
}
