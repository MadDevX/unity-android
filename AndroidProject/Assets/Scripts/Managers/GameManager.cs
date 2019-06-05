using Assets.Scripts.StateMachines;
using System.Collections;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{   
    public float GameStartTime { get; private set; }
    private GameStateMachine _gameStateManager;
    private LobbyManager _lobbyManager;
    private ConnectionStateMachine _connManager;
    private NetworkedGameManager _netGameManager;
    private GameSettings _gameSettings;

    private Coroutine _finishCor = null;
    private Coroutine _countdownCor = null;

    [Inject]
    public void Construct(GameStateMachine manager, LobbyManager lobbyManager, ConnectionStateMachine connManager, NetworkedGameManager netGameManager, GameSettings gameSettings)
    {
        _gameStateManager = manager;
        _lobbyManager = lobbyManager;
        _connManager = connManager;
        _netGameManager = netGameManager;
        _gameSettings = gameSettings;
    }

    private void Awake()
    {
        _connManager.SubscribeToInit(ConnectionState.Client, StartLobbyOnInitConnection);
        _connManager.SubscribeToInit(ConnectionState.Host, StartLobbyOnInitConnection);
        _connManager.SubscribeToInit(ConnectionState.Server, StartLobbyOnInitConnection);

        _connManager.SubscribeToDispose(ConnectionState.Server, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Host, ResetGame);
        _connManager.SubscribeToDispose(ConnectionState.Client, ResetGame);

        _gameStateManager.SubscribeToInit(GameState.Countdown, SetStartTime);
    }

    private void OnDestroy()
    {        
        _connManager.UnsubscribeFromInit(ConnectionState.Client, StartLobbyOnInitConnection);
        _connManager.UnsubscribeFromInit(ConnectionState.Host, StartLobbyOnInitConnection);
        _connManager.UnsubscribeFromInit(ConnectionState.Server, StartLobbyOnInitConnection);

        _connManager.UnsubscribeFromDispose(ConnectionState.Server, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Host, ResetGame);
        _connManager.UnsubscribeFromDispose(ConnectionState.Client, ResetGame);

        _gameStateManager.UnsubscribeFromInit(GameState.Countdown, SetStartTime);
    }

    private void Update()
    {
        if(_gameStateManager.State != GameState.Menu && _connManager.State == ConnectionState.Null)
        {
            ResetGame();
        }
    }

    public void StartCountdown()
    {
        _netGameManager.StartCountdown();
        _countdownCor = StartCoroutine(CountdownCoroutine());
    }

    private void StartLobby()
    {
        _netGameManager.StartLobby();
    }

    /// <summary>
    /// Only used to set initial state just after connection.
    /// </summary>
    private void StartLobbyOnInitConnection()
    {
        _gameStateManager.SetState(GameState.Lobby, new GameStateEventArgs(_lobbyManager.playerCount));
    }

    private void ResetGame()
    {
        _gameStateManager.SetState(GameState.Finished, new GameStateEventArgs(_lobbyManager.playerCount));
        _gameStateManager.SetState(GameState.Menu, new GameStateEventArgs(_lobbyManager.playerCount));
    }

    public void FinishGame()
    {
        if(_finishCor == null && _gameStateManager.State != GameState.Finished)
        {
            _netGameManager.FinishGame();
            _finishCor = StartCoroutine(FinishCoroutine());
        }
    }

    private IEnumerator FinishCoroutine()
    {
        yield return new WaitForSeconds(_gameSettings.finishDelay);
        StartLobby();
        _finishCor = null;
    }

    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(_gameSettings.countdownDelay);
        _netGameManager.StartGame();
        _countdownCor = null;
    }

    private void SetStartTime(GameStateEventArgs e)
    {
        GameStartTime = Time.time + _gameSettings.countdownDelay;
    }


}
