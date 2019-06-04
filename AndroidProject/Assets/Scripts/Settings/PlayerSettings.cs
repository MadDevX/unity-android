using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public float movementSpeed = 4.0f;
    public float runMult = 2.0f;
    public float lerpFactor = 0.3f;
    public float accelerationRate = 0.1f;
    public Vector3 offset = new Vector3(0,0.5f,0);
    public float bulletSpeed = 6;
    public float maxSpeed;
    public float boostMaxError;
    public float boostMaxValue;
}
