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

    [Inject]
    public void Construct(ServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    private void Awake()
    {
        _textBox = GetComponent<Text>();
        _serviceProvider.OnLocalPlayerChanged += UpdateId;
    }
    
    private void UpdateId(Player player)
    {
        _textBox.text = "Player " + player.GetComponent<NetworkIdentity>().netId;
    }
}
