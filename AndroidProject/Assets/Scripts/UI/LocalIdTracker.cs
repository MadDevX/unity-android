using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class LocalIdTracker : MonoBehaviour
{
    private Text _textBox;
    private ServiceProvider _serviceProvider;
    private Player _prevPlayer = null;

    [Inject]
    public void Construct(ServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    private void Awake()
    {
        _textBox = GetComponent<Text>();
        _serviceProvider.OnLocalPlayerChanged += TrackChanges;
    }
    
    private void TrackChanges(Player player)
    {
        if(_prevPlayer != null)
        {
            _prevPlayer.OnNicknameChangedEvent -= UpdateNick;
        }
        player.OnNicknameChangedEvent += UpdateNick;
        _prevPlayer = player;
    }

    private void UpdateNick(string nick)
    {
        _textBox.text = nick;
    }
}
