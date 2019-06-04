using Assets.Scripts.StateMachines;
using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [SerializeField]
    private ServiceProvider _serviceProvider;
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Track _environment;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private EnvironmentSettings _environmentSettings;
    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private PrefabManager _prefabManager;
    [SerializeField]
    private LobbyManager _lobbyManager;
    [SerializeField]
    private NetworkedGameManager _netGameManager;
    [SerializeField]
    private GameSettings _gameSettings;


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
        Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
    }
}