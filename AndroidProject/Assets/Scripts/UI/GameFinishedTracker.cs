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

    [Inject]
    public void Construct(LobbyManager lobbyManager, ScoreManager scoreManager, MatchCycleManager matchCycleManager)
    {
        _lobbyManager = lobbyManager;
        _scoreManager = scoreManager;
        _matchCycleManager = matchCycleManager;
    }

    private void Awake()
    {
        _textBox = GetComponent<Text>();

        _scoreManager.OnPlayerWon += SetFinishText;
        _matchCycleManager.OnAllPlayersDied += SetFinishTextAllDied;
    }

    private void OnDestroy()
    {
        _scoreManager.OnPlayerWon -= SetFinishText;
        _matchCycleManager.OnAllPlayersDied -= SetFinishTextAllDied;
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

    private void SetFinishTextAllDied()
    {
        _textBox.text = "Match Finished!\nNo one survived...";
    }
}
