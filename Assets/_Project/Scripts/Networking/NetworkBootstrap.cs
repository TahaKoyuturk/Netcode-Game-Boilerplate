using System.Threading.Tasks;
using Studio.Core.Networking;
using Studio.Core.Services;
using Unity.Netcode;
using NetworkConfig = Studio.Data.NetworkConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class NetworkBootstrap : MonoBehaviour
    {
        [SerializeField] private NetworkConfig networkConfig;

        private LobbyManager _lobbyManager;
        private RelayManager _relayManager;
        private NetworkManagerWrapper _networkWrapper;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        private async void Awake()
        {
            if (networkConfig == null)
            {
                var gameManager = Studio.Core.GameManager.Instance;
                networkConfig = gameManager?.Context?.Config?.NetworkConfig;
            }

            await InitializeServicesAsync();
        }

        public async Task InitializeServicesAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            if (networkConfig == null)
            {
                Debug.LogError("NetworkConfig is not assigned.");
                return;
            }

            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            _relayManager = new RelayManager(networkConfig);
            _lobbyManager = new LobbyManager(networkConfig, _relayManager);
            _networkWrapper = new NetworkManagerWrapper();

            ServiceLocator.Register<RelayManager>(_relayManager);
            ServiceLocator.Register<LobbyManager>(_lobbyManager);
            ServiceLocator.Register<NetworkManagerWrapper>(_networkWrapper);
            ServiceLocator.Register<INetworkHostService>(new NetworkHostService(_lobbyManager, _networkWrapper));

            if (NetworkManager.Singleton != null)
            {
                _networkWrapper.BindCallbacks();
            }

            _isInitialized = true;
        }

        public async Task<bool> HostGameAsync(string lobbyName)
        {
            await InitializeServicesAsync();
            await _lobbyManager.CreateLobbyAsync(lobbyName);
            return _networkWrapper.StartHost();
        }

        public async Task<bool> JoinGameAsync(string lobbyId)
        {
            await InitializeServicesAsync();
            await _lobbyManager.JoinLobbyAsync(lobbyId);
            return _networkWrapper.StartClient();
        }

        private void OnDestroy()
        {
            _networkWrapper?.UnbindCallbacks();
        }
    }
}
