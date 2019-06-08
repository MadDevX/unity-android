using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServiceProvider : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public MyNetworkManager networkManager;
    public MyNetworkDiscovery networkDiscovery;
    public Transform lobbySpawnPoint;

    public event Action<Player> OnLocalPlayerChanged;

    private Player _player;

    public Player Player
    {
        get
        {
            return _player;
        }

        set
        {
            _player = value;
            OnLocalPlayerChanged?.Invoke(_player);
        }
    }
}
