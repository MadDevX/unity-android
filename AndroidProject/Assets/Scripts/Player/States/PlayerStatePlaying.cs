using Assets.Scripts.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStatePlaying : PlayerState
{
    enum Direction
    {
        Null,
        Left,
        Right
    }

    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;
    private GameStateMachine _gameStateMachine;
    private GameManager _gameManager;
    private PlayerSettings _playerSettings;

    private bool _startBoost;
    private float _startBoostValue;

    private Direction _pendingTurn = Direction.Null;
    private Direction PendingTurn
    {
        get
        {
            return _pendingTurn;
        }
        set
        {
            _pendingTurn = value;
            _pendingTimer = 0.0f;
        }
    }
    private float _pendingTimer = 0.0f;

    [Inject]
    public void Construct(GameStateMachine gameStateMachine, GameManager gameManager, PlayerSettings playerSettings)
    {
        _gameStateMachine = gameStateMachine;
        _gameManager = gameManager;
        _playerSettings = playerSettings;
    }

    public override void Tick()
    {
        ProcessInput(_playerMovement, _playerShooting, _gameManager.GameStartTime);
        ExecutePendingTurn(_playerMovement);
        _pendingTimer += Time.deltaTime;
        ResetPendingTurn();
    }

    protected override void SetupReferences()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    protected override void Dispose()
    {
        _playerMovement.IsRunning = false;
    }

    protected override PlayerStates GetState()
    {
        return PlayerStates.Playing;
    }

    private void ResetPendingTurn()
    {
        if (_pendingTimer > _playerSettings.directionStickTime) PendingTurn = Direction.Null;
    }

    #region PlayerInput
    private Vector2 _touchOrigin = -Vector2.one;

    private void ProcessInput(PlayerMovement charMovement, PlayerShooting playerShooting, float gameStartTime)
    {
        if (_gameStateMachine.State == GameState.Started)
        {
            if (_startBoost) ApplyBoost(charMovement);
            SetMovement(charMovement);
            SwitchLanes(charMovement);
            Shoot(playerShooting);
        }
        else if(_gameStateMachine.State == GameState.Countdown)
        {
            StartingInput(charMovement, gameStartTime);
        }
    }

    private void SetMovement(PlayerMovement charMovement)
    {
        if (Input.GetButton("Fire1"))
        {
            charMovement.IsRunning = true;
        }
        else
        {
            charMovement.IsRunning = false;
        }
    }

    private void Shoot(PlayerShooting playerShooting)
    {
        if (Input.GetKeyDown(KeyCode.Space)) playerShooting.CmdFire();
    }

    #region Starting Boost
    private void StartingInput(PlayerMovement playerMovement, float gameStartTime)
    {
        if(Input.GetButtonDown("Fire1") && _startBoost == false)
        {
            var result = Mathf.Abs(Time.time - gameStartTime);
            Debug.Log($"Boost error: {result}");
            if(result < _playerSettings.boostMaxError)
            {
                var mult = (_playerSettings.boostMaxError - result) / _playerSettings.boostMaxError;
                _startBoostValue = mult * _playerSettings.boostMaxValue;
            }
            else
            {
                _startBoostValue = 0.0f;
            }
            _startBoost = true;
        }
    }

    private void ApplyBoost(PlayerMovement playerMovement)
    {
        playerMovement.ApplyBoost(_startBoostValue);
        _startBoost = false;
    }
    #endregion

    private void ExecutePendingTurn(PlayerMovement charMovement)
    {
        if (_gameStateMachine.State == GameState.Started)
        {
            switch (PendingTurn)
            {
                case Direction.Left:
                    if (charMovement.TurnLeft())
                    {
                        Debug.Log("Left stick");
                        PendingTurn = Direction.Null;
                    }
                    break;
                case Direction.Right:
                    if (charMovement.TurnRight())
                    {
                        Debug.Log("Right stick");
                        PendingTurn = Direction.Null;
                    }
                    break;
            }
        }
    }

    private void SwitchLanes(PlayerMovement charMovement)
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (charMovement.TurnLeft()) PendingTurn = Direction.Null;
            else PendingTurn = Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (charMovement.TurnRight()) PendingTurn = Direction.Null;
            else PendingTurn = Direction.Right;
        }
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
                    if (x > 0)
                    {
                        if (charMovement.TurnRight()) _pendingTurn = Direction.Null;
                        else _pendingTurn = Direction.Right;
                    }
                    else 
                    {
                        if (charMovement.TurnLeft()) _pendingTurn = Direction.Null;
                        else _pendingTurn = Direction.Left;
                    }
                }
                else
                {
                    //Boost logic?
                }
            }
        }
#endif
    }
    #endregion
}
