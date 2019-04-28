using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _touchOrigin = -Vector2.one;

    public void ProcessInput(CharacterMovement charMovement)
    {
        SetMovement(charMovement);
        SwitchLanes(charMovement);
    }

    void SetMovement(CharacterMovement charMovement)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            charMovement.IsRunning = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            charMovement.IsRunning = false;
        }
    }

    void SwitchLanes(CharacterMovement charMovement)
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.A)) charMovement.TurnLeft();
        if (Input.GetKeyDown(KeyCode.D)) charMovement.TurnRight();
#else
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
            {
                _touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && _touchOrigin.x >= 0.0f)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - _touchOrigin.x;
                float y = touchEnd.y - _touchOrigin.y;
                _touchOrigin.x = -1.0f;
                if (Mathf.Abs(x) > Mathf.Abs(y) && Mathf.Abs(x) > Screen.width/15)
                {
                    Debug.Log(x);
                    if (x > 0) charMovement.TurnRight();
                    else charMovement.TurnLeft();
                }
                else
                {
                    //Boost logic?
                }
            }
        }
#endif
    }
}
