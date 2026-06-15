using System;
using Studio.Core.Services;
using Unity.Netcode;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class NetworkManagerWrapper : IService
    {
        public bool IsHost => NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost;
        public bool IsClient => NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient;
        public bool IsConnected => NetworkManager.Singleton != null &&
                                   (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient);

        public event Action<bool> OnConnected;
        public event Action OnDisconnected;

        public void BindCallbacks()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        }

        public void UnbindCallbacks()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }

        public bool StartHost()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager.Singleton is null.");
                return false;
            }

            var started = NetworkManager.Singleton.StartHost();
            if (started)
            {
                OnConnected?.Invoke(true);
                Studio.Core.Events.EventBus.Publish(new NetworkConnectedEvent(true));
            }

            return started;
        }

        public bool StartClient()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager.Singleton is null.");
                return false;
            }

            var started = NetworkManager.Singleton.StartClient();
            if (started)
            {
                OnConnected?.Invoke(false);
                Studio.Core.Events.EventBus.Publish(new NetworkConnectedEvent(false));
            }

            return started;
        }

        public void Shutdown()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.Shutdown();
            OnDisconnected?.Invoke();
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer && clientId == NetworkManager.Singleton.LocalClientId)
            {
                OnConnected?.Invoke(true);
            }
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                OnDisconnected?.Invoke();
            }
        }
    }
}
