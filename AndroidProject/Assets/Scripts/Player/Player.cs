using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerNetworkPresence _playerNetworkPresence;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerNetworkPresence = GetComponent<PlayerNetworkPresence>();
    }


    public void Kill()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Player was killed!");
        }
    }

    public void PaintRandom()
    {
        if (isLocalPlayer)
        {
            CmdPaint();
        }
    }

    [Command]
    private void CmdPaint()
    {
        Color randomColor = Random.ColorHSV();
        _spriteRenderer.color = randomColor;
    }

    public void Respawn()
    {
        _playerNetworkPresence.RpcRespawn();
    }
}
