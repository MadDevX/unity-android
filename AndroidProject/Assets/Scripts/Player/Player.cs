using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Kill()
    {
        Debug.Log("Player was killed!");
    }

    public void PaintRandom()
    {
        Color randomColor = Random.ColorHSV();
        //randomColor.a = 1.0f;
        _spriteRenderer.color = randomColor;
    }
}
