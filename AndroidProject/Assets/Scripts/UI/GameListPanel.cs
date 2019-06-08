using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameListPanel : UIPanel
{
    public Transform listHolder;
    private MyNetworkDiscovery _netDiscovery;
    private PrefabManager _prefabManager;
    private DiContainer _container;
    private List<JoinGameButton> _buttons = new List<JoinGameButton>();

    [Inject]
    public void Construct(ServiceProvider provider, PrefabManager prefabManager, DiContainer container)
    {
        _netDiscovery = provider.networkDiscovery;
        _prefabManager = prefabManager;
        _container = container;
    }

    private void Awake()
    {
        _netDiscovery.OnReceivedBroadcastEvent += CreateButton;
    }

    private void OnDestroy()
    {
        _netDiscovery.OnReceivedBroadcastEvent -= CreateButton;
    }

    private void CreateButton(string fromAddress, string data)
    {
        var info = ParseData(data);
        var gameData = new JoinGameButton.GameData(info[0], fromAddress);
        foreach(var b in _buttons)
        {
            if (b.IsDataMatching(gameData)) return;
        }
        var button = _container.InstantiatePrefab(_prefabManager.joinGameButton, listHolder).GetComponent<JoinGameButton>();
        button.InitButton(gameData);
        _buttons.Add(button);
    }

    private string[] ParseData(string data)
    {
        return data.Split(';');
    }

    public void ClearButtons()
    {
        foreach(var button in _buttons)
        {
            Destroy(button.gameObject);
        }

        _buttons.Clear();
    }
}
