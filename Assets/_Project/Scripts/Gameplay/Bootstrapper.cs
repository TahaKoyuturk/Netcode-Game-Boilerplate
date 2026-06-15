using Studio.Core;
using Studio.Core.Save;
using Studio.Core.SceneLoading;
using Studio.Core.Services;
using Studio.Core.Tick;
using Studio.Data;
using Studio.Gameplay.States;
using Studio.Managers;
using Studio.Systems.Input;
using Studio.Systems.Loading;
using Studio.Systems.Settings;
using Studio.Systems.Tween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Studio.Gameplay
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        public static Bootstrapper Instance { get; private set; }

        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private InputActionAsset inputActionsAsset;

        private async void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (gameConfig == null)
            {
                Debug.LogError("GameConfig is not assigned on Bootstrapper.");
                return;
            }

            RegisterCoreServices();
            RegisterSystems();
            RegisterManagers();
            InitializeManagers();
            RegisterGameplayStates();
            await LoadMainMenuAsync();
        }

        private void RegisterCoreServices()
        {
            ServiceLocator.Register<ISaveService>(new SaveService());
            ServiceLocator.Register<ISceneLoader>(new SceneLoader());
            ServiceLocator.Register<ITweenService>(new NullTweenService());
            ServiceLocator.Register(new LoadingPipeline());

            var gameManager = GetOrAddComponent<GameManager>();
            gameManager.Initialize(gameConfig);
            ServiceLocator.Register(gameManager);
        }

        private void RegisterSystems()
        {
            var saveService = ServiceLocator.Get<ISaveService>();
            var settingsSystem = new SettingsSystem(saveService, gameConfig.SettingsConfig);
            ServiceLocator.Register(settingsSystem);

            if (inputActionsAsset != null)
            {
                ServiceLocator.Register(new InputBindingService(saveService, inputActionsAsset));
            }
        }

        private void RegisterManagers()
        {
            var saveService = ServiceLocator.Get<ISaveService>();
            var settingsSystem = ServiceLocator.Get<SettingsSystem>();
            var sceneLoader = ServiceLocator.Get<ISceneLoader>();
            var pipeline = ServiceLocator.Get<LoadingPipeline>();

            ServiceLocator.Register(new UIManager());
            ServiceLocator.Register(new AudioManager(gameConfig.AudioConfig, settingsSystem));
            ServiceLocator.Register(new PoolManager());
            ServiceLocator.Register(new CurrencyManager(saveService, gameConfig.EconomyConfig));
            ServiceLocator.Register(new SettingsManager(settingsSystem));
            ServiceLocator.Register(new NotificationManager());
            ServiceLocator.Register(new LoadingManager(pipeline, sceneLoader));
            ServiceLocator.Register(new PopupManager());
        }

        private void InitializeManagers()
        {
            var tickSystem = ServiceLocator.Get<TickSystem>();
            var managers = new IManager[]
            {
                ServiceLocator.Get<UIManager>(),
                ServiceLocator.Get<AudioManager>(),
                ServiceLocator.Get<PoolManager>(),
                ServiceLocator.Get<CurrencyManager>(),
                ServiceLocator.Get<SettingsManager>(),
                ServiceLocator.Get<NotificationManager>(),
                ServiceLocator.Get<LoadingManager>(),
                ServiceLocator.Get<PopupManager>()
            };

            for (var i = 0; i < managers.Length; i++)
            {
                managers[i].Initialize();
                if (managers[i] is ITickable tickable)
                {
                    tickSystem.Register(tickable);
                }
            }
        }

        private void RegisterGameplayStates()
        {
            var gameManager = ServiceLocator.Get<GameManager>();
            gameManager.StateMachine.RegisterState(new MenuState());
            gameManager.StateMachine.RegisterState(new LobbyState());
            gameManager.StateMachine.RegisterState(new GameplayState());
            gameManager.StateMachine.RegisterState(new PauseState());
            gameManager.StateMachine.RegisterState(new GameOverState());
            gameManager.StateMachine.ChangeState(MenuState.StateId);
        }

        private async System.Threading.Tasks.Task LoadMainMenuAsync()
        {
            var loadingManager = ServiceLocator.Get<LoadingManager>();
            await loadingManager.LoadSceneAsync(gameConfig.MainMenuScene, additive: true);
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}
