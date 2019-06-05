using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class GameFinishedTracker : MonoBehaviour
{
    private Text _textBox;

    private LobbyManager _lobbyManager;
    private ScoreManager _scoreManager;
    private MatchCycleManager _matchCycleManager;
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(LobbyManager lobbyManager, ScoreManager scoreManager, MatchCycleManager matchCycleManager, GameStateMachine gameStateMachine)
    {
        _lobbyManager = lobbyManager;
        _scoreManager = scoreManager;
        _matchCycleManager = matchCycleManager;
        _gameStateMachine = gameStateMachine;
    }

    private void Awake()
    {
        _textBox = GetComponent<Text>();

        _scoreManager.OnPlayerWon += SetFinishText;
        _gameStateMachine.SubscribeToInit(GameState.Finished, SetFinishTextAllDied);
    }

    private void OnDestroy()
    {
        _scoreManager.OnPlayerWon -= SetFinishText;
        _gameStateMachine.UnsubscribeFromInit(GameState.Finished, SetFinishTextAllDied);
    }

    private void SetFinishText(NetworkInstanceId netId)
    {
        foreach(var player in _lobbyManager.GetActivePlayers())
        {
            if (player.isLocalPlayer)
            {
                if(netId == player.GetComponent<NetworkIdentity>().netId)
                {
                    _textBox.text = "Match Finished!\nYou won!";
                }
                else
                {
                    _textBox.text = "Match Finished!\nYou lost...";
                }
                break;
            }
        }
    }

    private void SetFinishTextAllDied(GameStateEventArgs e)
    {
        if (e.someoneWon == false)
        {
            _textBox.text = "Match Finished!\nNo one survived...";
        }
    }
}
