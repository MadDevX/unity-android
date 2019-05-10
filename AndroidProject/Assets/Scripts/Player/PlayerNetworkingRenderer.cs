using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkingRenderer : NetworkBehaviour
{
    [SyncVar(hook = "OnColorChanged")]
    public Color _color;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    public void PaintRandom()
    {
        if (!isLocalPlayer) return;

        CmdPaint();
    }

    private void OnColorChanged(Color color)
    {
        _color = color;
        _spriteRenderer.color = _color;
    }

    [Command]
    private void CmdPaint()
    {
        _color = Random.ColorHSV();
    }
}
