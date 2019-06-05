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
    
    private ScoreManager _scoreManager;
    private GameStateMachine _gameStateMachine;
    private ServiceProvider _serviceProvider;

    [Inject]
    public void Construct(ScoreManager scoreManager, GameStateMachine gameStateMachine, ServiceProvider provider)
    {
        _scoreManager = scoreManager;
        _gameStateMachine = gameStateMachine;
        _serviceProvider = provider;
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
        if(netId.Value == _serviceProvider.player.GetComponent<NetworkIdentity>().netId.Value)
        {
            _textBox.text = "Match Finished!\nYou won!";
        }
        else
        {
            _textBox.text = "Match Finished!\nYou lost...";
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
