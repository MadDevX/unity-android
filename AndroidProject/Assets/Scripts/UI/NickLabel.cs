using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class NickLabel : MonoBehaviour
{
    private Text _textBox;
    private Player _player;
    private PlayerMovement _playerMovement;
    private Camera _cam;
    private UIManager _uiManager;

    private void Awake()
    {
        _textBox = GetComponent<Text>();
    }
    
    public void Init(Player player, Camera camera, UIManager uiManager)
    {
        _player = player;
        _playerMovement = player.GetComponent<PlayerMovement>();
        _cam = camera;
        _uiManager = uiManager;
        UpdateNick(player.Nickname);
        _player.OnNicknameChangedEvent += UpdateNick;
    }

    private void OnDestroy()
    {
        _player.OnNicknameChangedEvent -= UpdateNick;
    }

    private void FixedUpdate()
    {
        Vector2 position = _cam.WorldToScreenPoint(_playerMovement.GetPosition());
        transform.position = position + _uiManager.relativeUIOffset(_uiManager.nickLabelOffset);
    }

    private void UpdateNick(string nick)
    {
        _textBox.text = nick;
    }

}
