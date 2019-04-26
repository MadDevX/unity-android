using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServiceProvider : MonoBehaviour
{
    private static ServiceProvider _instance;
    public CinemachineVirtualCamera vCam;
    public Track track;
    public NetworkManager networkManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameManager instance!");
        }
    }

    public static ServiceProvider Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Object tried calling GameManager before its creation");
                return null;
            }
            return _instance;
        }
    }
    
}
