using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkPresence : NetworkBehaviour
{
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody2D;
    private List<Transform> _spawnPositions;

    public override void OnStartLocalPlayer()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _sprite.color = Color.blue;
        GameManager.Instance.vCam.Follow = transform;
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            _spawnPositions = NetworkManager.singleton.startPositions; //level generates several times (transforms can change often)
            _rigidbody2D.position = _spawnPositions[Random.Range(0, _spawnPositions.Count - 1)].position;
        }
    }
}
