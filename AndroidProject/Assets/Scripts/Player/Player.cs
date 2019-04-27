using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerNetworkPresence _playerNetworkPresence;

    [SyncVar(hook = "OnColorChanged")]
    public Color _color;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerNetworkPresence = GetComponent<PlayerNetworkPresence>();
        if (isLocalPlayer == false)
        {
            OnColorChanged(_color);
        }
    }

    void OnColorChanged(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Kill()
    {
        if (isLocalPlayer == false) return;

        Debug.Log("Player was killed!");
    }

    public void PaintRandom()
    {
        if (isLocalPlayer == false) return;

        CmdPaint();
    }

    [Command]
    private void CmdPaint()
    {
        _color = Random.ColorHSV();
    }

    public void Respawn()
    {
        if (!isServer) return;

        _playerNetworkPresence.RpcRespawn();
    }
}
