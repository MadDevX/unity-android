using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class PlayerRespawn : NetworkBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private List<Transform> _spawnPositions;
    private PlayerMovement _charMovement;

    public override void OnStartLocalPlayer()
    {
        _charMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
        _rigidbody2D.position = newPos;
        _rigidbody2D.velocity = Vector2.zero;
        _charMovement.SetLane((int)spawnPoint.position.x);
    }
}
