using System.Collections.Generic;
using Studio.Core.Events;
using Unity.Netcode;

namespace Studio.Networking
{
    public sealed class ReadyCheckSystem : NetworkBehaviour
    {
        private readonly HashSet<ulong> _readyClients = new();

        public bool AllReady => IsServer && _readyClients.Count >= ConnectedClientsCount() && ConnectedClientsCount() > 0;

        public override void OnNetworkSpawn()
        {
            if (IsServer && NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            }
        }

        private int ConnectedClientsCount()
        {
            return NetworkManager.Singleton != null
                ? NetworkManager.Singleton.ConnectedClientsIds.Count
                : 0;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetReadyServerRpc(ulong clientId, bool ready)
        {
            if (ready)
            {
                _readyClients.Add(clientId);
            }
            else
            {
                _readyClients.Remove(clientId);
            }

            if (AllReady)
            {
                NotifyAllReadyClientRpc();
            }
        }

        [ClientRpc]
        private void NotifyAllReadyClientRpc()
        {
            EventBus.Publish(new AllPlayersReadyEvent());
        }

        public void SetLocalReady(bool ready)
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId, ready);
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            _readyClients.Remove(clientId);
        }
    }
}
