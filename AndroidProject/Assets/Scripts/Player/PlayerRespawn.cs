using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerRespawn : NetworkBehaviour
{
    private List<Transform> _spawnPositions;
    private PlayerMovement _playerMovement;

    public override void OnStartLocalPlayer()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Respawn()
    {
        if (!isServer) return;
        RpcRespawn();
    }

    public void Respawn(int i)
    {
        if (!isServer) return;
        RpcRespawnAtSpawnPoint(i);
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            _spawnPositions = NetworkManager.singleton.startPositions; //level generates several times (transforms can change often)
            RespawnAtSpawnPoint(_spawnPositions[Random.Range(0, _spawnPositions.Count - 1)]);
        }
    }

    [ClientRpc]
    private void RpcRespawnAtSpawnPoint(int i)
    {
        if (isLocalPlayer)
        {
            _spawnPositions = NetworkManager.singleton.startPositions; //level generates several times (transforms can change often)
            RespawnAtSpawnPoint(_spawnPositions[i]);
        }
    }

    private void RespawnAtSpawnPoint(Transform spawnPoint)
    {
        Vector2 newPos = spawnPoint.position;
        _playerMovement.SetPosition(newPos);
        _playerMovement.ResetVelocity();
        _playerMovement.SetLane((int)spawnPoint.position.x);
    }
}
