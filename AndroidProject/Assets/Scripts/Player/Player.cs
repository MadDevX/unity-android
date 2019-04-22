using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Kill()
    {
        gameObject.SetActive(false);
    }

}
