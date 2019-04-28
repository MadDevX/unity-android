using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = "OnColorChanged")]
    public Color _color;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerRespawn _playerNetworkPresence;
    private PlayerInput _playerInput;
    private CharacterMovement _charMovement;
    
    private PlayerState _state;
    private CinemachineVirtualCamera _vCam;

    [Inject]
    public void Construct(ServiceProvider provider)
    {
        _vCam = provider.vCam;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerNetworkPresence = GetComponent<PlayerRespawn>();
        _playerInput = GetComponent<PlayerInput>();
        _charMovement = GetComponent<CharacterMovement>();
    }

    public override void OnStartLocalPlayer()
    {
        _vCam.Follow = transform;
        _color = _spriteRenderer.color;
    }

    private void Start()
    {
        if(!isLocalPlayer)
        {
            OnColorChanged(_color);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _playerInput.ProcessInput(_charMovement);
    }

    void OnColorChanged(Color color)
    {
        _color = color;
        _spriteRenderer.color = _color;
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

    public void ChangeState(PlayerState state)
    {

    }
}
