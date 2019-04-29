using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerCountUpdated")]
    public int playerCount;

    public event Action<int> OnPlayerCountChanged;


    private void OnPlayerCountUpdated(int pCount)
    {
        playerCount = pCount;
        OnPlayerCountChanged?.Invoke(playerCount);
    }
}
