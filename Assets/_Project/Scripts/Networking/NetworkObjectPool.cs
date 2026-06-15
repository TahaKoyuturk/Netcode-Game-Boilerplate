using System.Collections.Generic;
using Studio.Core.Services;
using Studio.Managers;
using Studio.Systems.Pooling;
using Unity.Netcode;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class NetworkObjectPool : NetworkBehaviour
    {
        [SerializeField] private PoolConfig poolConfig;

        private PoolManager _poolManager;
        private readonly Dictionary<ulong, PooledNetworkBehaviour> _spawned = new();

        public override void OnNetworkSpawn()
        {
            if (!IsServer || poolConfig == null)
            {
                return;
            }

            if (ServiceLocator.TryGet<PoolManager>(out _poolManager))
            {
                _poolManager.CreatePool<PooledNetworkBehaviour>(poolConfig);
            }
        }

        public void SpawnForClient(ulong clientId, Vector3 position, Quaternion rotation)
        {
            if (!IsServer || _poolManager == null || poolConfig == null)
            {
                return;
            }

            if (!_poolManager.TryGetPool<PooledNetworkBehaviour>(poolConfig.PoolId, out var pool))
            {
                return;
            }

            var instance = pool.Spawn(position, rotation);
            if (instance == null)
            {
                return;
            }

            var networkObject = instance.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnAsPlayerObject(clientId);
                _spawned[clientId] = instance;
            }
        }

        public void DespawnForClient(ulong clientId)
        {
            if (!IsServer || !_spawned.TryGetValue(clientId, out var instance) || poolConfig == null)
            {
                return;
            }

            if (_poolManager.TryGetPool<PooledNetworkBehaviour>(poolConfig.PoolId, out var pool))
            {
                var networkObject = instance.GetComponent<NetworkObject>();
                networkObject?.Despawn();
                pool.Despawn(instance);
            }

            _spawned.Remove(clientId);
        }
    }
}
