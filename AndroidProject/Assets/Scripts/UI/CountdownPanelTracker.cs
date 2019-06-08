﻿using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CountdownPanelTracker : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;
    private GameManager _gameManager;
    private Text _textBox;

    [Inject]
    public void Construct(GameStateMachine gameStateMachine, GameManager gameManager)
    {
        _gameStateMachine = gameStateMachine;
        _gameManager = gameManager;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _textBox = GetComponent<Text>();

        _gameStateMachine.SubscribeToInit(GameState.Countdown, ShowPanel);
        _gameStateMachine.SubscribeToDispose(GameState.Countdown, HidePanel);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        float remaining = _gameManager.GameStartTime - Time.time;
        if (remaining > 0.0f)
        {
            SetText(((int)remaining + 1).ToString());
        }
        else
        {
            SetText("GO!");
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        _gameStateMachine.UnsubscribeFromInit(GameState.Countdown, ShowPanel);
        _gameStateMachine.UnsubscribeFromDispose(GameState.Countdown, HidePanel);
    }

    private void ShowPanel(GameStateEventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void HidePanel(GameStateEventArgs e)
    {
        StartCoroutine(HidePanelCoroutine());
    }

    private IEnumerator HidePanelCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }

    private void SetText(string text)
    {
        _textBox.text = text;
    }
}
