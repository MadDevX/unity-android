using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkingRenderer : NetworkBehaviour
{
    [SyncVar(hook = "OnColorChanged")]
    public Color _color;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerNetworkingLobby _playerLobby;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerLobby = GetComponent<PlayerNetworkingLobby>();
    }

    public override void OnStartLocalPlayer()
    {
        _color = _spriteRenderer.color;
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            OnColorChanged(_color);
        }
        else
        {
            _playerLobby.OnPlayerReset += ResetColor;
        }
    }

    private void OnDestroy()
    {
        _playerLobby.OnPlayerReset -= ResetColor;
    }

    public void PaintRandom()
    {
        if (!isLocalPlayer) return;

        CmdPaint(true);
    }

    private void ResetColor()
    {
        if (!isLocalPlayer) return;

        CmdPaint(false);
    }

    private void OnColorChanged(Color color)
    {
        _color = color;
        _spriteRenderer.color = _color;
    }

    [Command]
    private void CmdPaint(bool random)
    {
        if (random)
        {
            _color = Random.ColorHSV(0.0f, 1.0f, 0.5f, 0.75f, 1.0f, 1.0f);
        }
        else
        {
            _color = Color.white;
        }
    }
}
