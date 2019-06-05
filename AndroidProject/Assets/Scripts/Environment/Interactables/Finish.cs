using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class Finish : NetworkBehaviour
{
    private GameManager _gameManager = null;
    private ScoreManager _scoreManager;
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(GameManager gameManager, ScoreManager scoreManager, GameStateMachine gameStateMachine)
    {
        _gameManager = gameManager;
        _scoreManager = scoreManager;
        _gameStateMachine = gameStateMachine;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerRespawn = collision.GetComponent<PlayerRespawn>();
        if (_gameStateMachine.State != GameState.Finished && playerRespawn != null)
        {
            //playerRespawn.Respawn();
            _scoreManager.IncrementScore(playerRespawn.GetComponent<NetworkIdentity>().netId);
            _gameManager.FinishGame();
        }
    }
}
