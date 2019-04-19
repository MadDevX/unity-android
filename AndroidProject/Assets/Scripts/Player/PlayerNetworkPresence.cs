using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkPresence : NetworkBehaviour
{
    private SpriteRenderer _sprite;
    
    public override void OnStartLocalPlayer()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _sprite.color = Color.blue;
    }
}
