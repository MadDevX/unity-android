using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreListEntry : MonoBehaviour
{
    private Text _textBox;

    private void Awake()
    {
        _textBox = GetComponent<Text>();
    }

    public void Init(NetworkInstanceId netId, int score, bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            _textBox.text = string.Format("Player {0} (Me): {1}", netId, score);
        }
        else
        {
            _textBox.text = string.Format("Player {0}: {1}", netId, score);
        }
    }
}
