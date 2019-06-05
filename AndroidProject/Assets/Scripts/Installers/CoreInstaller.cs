using Assets.Scripts.StateMachines;
using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    public ServiceProvider _serviceProvider;
    public GridManager _gridManager;
    public GameManager _gameManager;
    public Track _environment;
    public UIManager _uiManager;
    public EnvironmentSettings _environmentSettings;
    public PlayerSettings _playerSettings;
    public PrefabManager _prefabManager;
    public LobbyManager _lobbyManager;
    public NetworkedGameManager _netGameManager;
    public GameSettings _gameSettings;
    public ScoreManager _scoreManager;
    public MatchCycleManager _matchCycleManager;


    public override void InstallBindings()
    {
        Container.Bind<GameStateMachine>().AsSingle();
        Container.Bind<ConnectionStateMachine>().AsSingle();
        Container.Bind<ServiceProvider>().FromInstance(_serviceProvider).AsSingle();
        Container.Bind<Track>().FromInstance(_environment).AsSingle();
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
        Container.Bind<GridManager>().FromInstance(_gridManager).AsSingle();
        Container.Bind<PlayerSettings>().FromInstance(_playerSettings).AsSingle();
        Container.Bind<EnvironmentSettings>().FromInstance(_environmentSettings).AsSingle();
        Container.Bind<PrefabManager>().FromInstance(_prefabManager).AsSingle();
        Container.Bind<LobbyManager>().FromInstance(_lobbyManager).AsSingle();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
        Container.Bind<NetworkedGameManager>().FromInstance(_netGameManager).AsSingle();
        Container.Bind<ScoreManager>().FromInstance(_scoreManager).AsSingle();
        Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
        Container.Bind<MatchCycleManager>().FromInstance(_matchCycleManager).AsSingle();
    }
}