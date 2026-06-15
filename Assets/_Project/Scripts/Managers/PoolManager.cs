using System.Collections.Generic;
using Studio.Core.Services;
using Studio.Systems.Pooling;
using UnityEngine;

namespace Studio.Managers
{
    public sealed class PoolManager : IManager
    {
        private readonly Dictionary<string, object> _pools = new();
        private Transform _poolRoot;

        public void Initialize()
        {
            var root = new GameObject("PoolRoot");
            Object.DontDestroyOnLoad(root);
            _poolRoot = root.transform;
        }

        public void Shutdown()
        {
            _pools.Clear();
            if (_poolRoot != null)
            {
                Object.Destroy(_poolRoot.gameObject);
            }
        }

        public ObjectPool<T> CreatePool<T>(PoolConfig config) where T : Component, IPoolable
        {
            var prefab = config.Prefab as T;
            if (prefab == null)
            {
                Debug.LogError($"Pool '{config.PoolId}' prefab must be of type {typeof(T).Name}.");
                return null;
            }

            var pool = new ObjectPool<T>(config.PoolId, prefab, _poolRoot, config.PrewarmCount, config.MaxSize);
            _pools[config.PoolId] = pool;
            return pool;
        }

        public bool TryGetPool<T>(string poolId, out ObjectPool<T> pool) where T : Component, IPoolable
        {
            if (_pools.TryGetValue(poolId, out var value) && value is ObjectPool<T> typedPool)
            {
                pool = typedPool;
                return true;
            }

            pool = null;
            return false;
        }
    }
}
