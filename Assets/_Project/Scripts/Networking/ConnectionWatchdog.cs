using Studio.Core.Events;
using EventBus = Studio.Core.Events.EventBus;
using Studio.Core.Services;
using Unity.Netcode;
using NetworkConfig = Studio.Data.NetworkConfig;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class ConnectionWatchdog : MonoBehaviour
    {
        [SerializeField] private NetworkConfig networkConfig;

        private NetworkManagerWrapper _networkWrapper;
        private float _timeoutTimer;
        private bool _isConnected;

        private void Start()
        {
            if (networkConfig == null)
            {
                var gm = Studio.Core.GameManager.Instance;
                networkConfig = gm?.Context?.Config?.NetworkConfig;
            }

            if (ServiceLocator.TryGet<NetworkManagerWrapper>(out _networkWrapper))
            {
                _networkWrapper.OnConnected += _ => _isConnected = true;
                _networkWrapper.OnDisconnected += HandleDisconnect;
            }
        }

        private void Update()
        {
            if (!_isConnected || networkConfig == null)
            {
                return;
            }

            if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsConnectedClient)
            {
                _timeoutTimer += Time.deltaTime;
                if (_timeoutTimer >= networkConfig.ConnectionTimeoutSeconds)
                {
                    HandleDisconnect();
                }
            }
            else
            {
                _timeoutTimer = 0f;
            }
        }

        private void HandleDisconnect()
        {
            _isConnected = false;
            EventBus.Publish(new ConnectionLostEvent("Connection timeout or disconnect."));
        }
    }
}
