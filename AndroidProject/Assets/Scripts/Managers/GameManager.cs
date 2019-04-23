using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public CinemachineVirtualCamera vCam;

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

    public static GameManager Instance
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
