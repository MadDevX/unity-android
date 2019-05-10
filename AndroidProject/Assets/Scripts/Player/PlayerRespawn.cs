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

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            _spawnPositions = NetworkManager.singleton.startPositions; //level generates several times (transforms can change often)
            var spawnPoint = _spawnPositions[Random.Range(0, _spawnPositions.Count - 1)];
            Vector2 newPos = spawnPoint.position;
            _rigidbody2D.position = newPos;
            _charMovement.SetLane((int)spawnPoint.position.x);
        }
    }
}
