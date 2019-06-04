using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadingPanelTracker : MonoBehaviour
{
    private MyNetworkManager _networkManager;

    [Inject]
    public void Construct(ServiceProvider serviceProvider)
    {
        _networkManager = serviceProvider.networkManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(_networkManager.ClientIsConnected)
        {
            gameObject.SetActive(false);
        }
    }
}
