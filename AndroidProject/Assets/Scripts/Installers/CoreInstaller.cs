using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [SerializeField]
    private ServiceProvider _serviceProvider;
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private Track _environment;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private EnvironmentSettings _environmentSettings;
    [SerializeField]
    private PlayerSettings _playerSettings;


    public override void InstallBindings()
    {
        Container.Bind<GameStateManager>().AsSingle();
        Container.Bind<ConnectionStateManager>().AsSingle();
        Container.Bind<ServiceProvider>().FromInstance(_serviceProvider).AsSingle();
        Container.Bind<Track>().FromInstance(_environment).AsSingle();
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
        Container.Bind<GridManager>().FromInstance(_gridManager).AsSingle();
        Container.Bind<PlayerSettings>().FromInstance(_playerSettings).AsSingle();
        Container.Bind<EnvironmentSettings>().FromInstance(_environmentSettings).AsSingle();
    }
}