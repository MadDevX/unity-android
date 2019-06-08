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

    public bool Init(NetworkInstanceId netId, int score, List<Player> players)
    {
        Player player = null;
        foreach(var p in players)
        {
            if (p.GetComponent<NetworkIdentity>().netId.Value == netId.Value)
            {
                player = p;
                break;
            }
        }
        if(player == null)
        {
            Debug.LogError("No player with matching netId!");
            return false;
        }
        if (player.isLocalPlayer)
        {
            _textBox.color = new Color(0.9f, 0.65f, 0.2f);
        }
        _textBox.text = string.Format("{0}: {1}", player.Nickname, score);
        return true;
    }
}
