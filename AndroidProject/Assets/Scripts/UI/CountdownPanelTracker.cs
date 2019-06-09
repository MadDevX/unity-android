using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CountdownPanelTracker : MonoBehaviour
{
    private Text _textBox;
    private Animator _anim;

    private GameStateMachine _gameStateMachine;
    private GameManager _gameManager;

    private int _prevTime = -1;

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
        _anim = GetComponent<Animator>();

        _gameStateMachine.SubscribeToInit(GameState.Countdown, ShowPanel);
        _gameStateMachine.SubscribeToDispose(GameState.Countdown, HidePanel);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        float remaining = _gameManager.GameStartTime - Time.time;
        if (remaining > 0.0f)
        {
            var time = ((int)remaining + 1);
            if (time != _prevTime)
            {
                SetText(((int)remaining + 1).ToString());
                _prevTime = time;
            }
        }
        else
        {
            if (_prevTime != 0)
            {
                SetText("GO!");
                _prevTime = 0;
            }
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
        _prevTime = -1;
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
        _anim.SetTrigger("Tick");
    }
}
