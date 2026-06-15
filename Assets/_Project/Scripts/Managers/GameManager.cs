using Studio.Core.Services;
using Studio.Core.Tick;
using Studio.Data;
using UnityEngine;
using GameStateMachine = Studio.Core.StateMachine.StateMachine;

namespace Studio.Core
{
    public sealed class GameManager : MonoBehaviour, IService
    {
        public static GameManager Instance { get; private set; }

        public GameStateMachine StateMachine { get; private set; }
        public GameContext Context { get; private set; }

        private TickSystem _tickSystem;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            StateMachine = new GameStateMachine();
        }

        public void Initialize(GameConfig config)
        {
            Context = new GameContext(config);
            _tickSystem = GetComponent<TickSystem>();
            if (_tickSystem == null)
            {
                _tickSystem = gameObject.AddComponent<TickSystem>();
            }

            if (!ServiceLocator.IsRegistered<TickSystem>())
            {
                ServiceLocator.Register<TickSystem>(_tickSystem);
            }
        }

        private void Update()
        {
            StateMachine?.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
