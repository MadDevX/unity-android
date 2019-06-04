using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Finish : NetworkBehaviour
{
    private GameManager _gameManager = null;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerRespawn = collision.GetComponent<PlayerRespawn>();
        if (playerRespawn != null)
        {
            //playerRespawn.Respawn();
            _gameManager.FinishGame();
        }
    }
}
