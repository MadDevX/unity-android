using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Player : NetworkBehaviour
{
    public StateEventManager<PlayerStates> stateManager = new StateEventManager<PlayerStates>();
    public PlayerState State { private get; set; }

    [SyncVar(hook = "OnColorChanged")]
    public Color _color;

    private SpriteRenderer _spriteRenderer;
    private PlayerRespawn _playerNetworkPresence;
    private PlayerMovement _charMovement;
    private PlayerShooting _playerShooting;
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
        _charMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();       
    }

    public override void OnStartLocalPlayer()
    {
        _vCam.Follow = transform;
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
            stateManager.SetState(PlayerStates.Playing);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        State.Tick();
    }

    void OnColorChanged(Color color)
    {
        _color = color;
        _spriteRenderer.color = _color;
    }

    public void Kill()
    {
        if (!isLocalPlayer) return;

        Debug.Log("Player was killed!");
    }

    public void PaintRandom()
    {
        if (!isLocalPlayer) return;

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
