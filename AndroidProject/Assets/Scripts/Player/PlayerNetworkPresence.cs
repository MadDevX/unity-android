﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerNetworkPresence : NetworkBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private List<Transform> _spawnPositions;
    private CharacterMovement _charMovement;

    private CinemachineVirtualCamera _vCam;

    [Inject]
    public void Construct(ServiceProvider provider)
    {
        _vCam = provider.vCam;
    }

    public override void OnStartLocalPlayer()
    {
        _charMovement = GetComponent<CharacterMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _vCam.Follow = transform;
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            _spawnPositions = NetworkManager.singleton.startPositions; //level generates several times (transforms can change often)
            RaceLane lane = _spawnPositions[Random.Range(0, _spawnPositions.Count - 1)].GetComponent<RaceLane>();
            Vector2 newPos = lane.transform.position;
            _rigidbody2D.position = newPos;
            _charMovement.SetLane(lane.GetOffset().x);
        }
    }
}
