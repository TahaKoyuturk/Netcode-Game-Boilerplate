using System.Collections.Generic;
using UnityEngine;

namespace Studio.Systems.Pooling
{
    public sealed class ObjectPool<T> where T : Component, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly int _maxSize;
        private readonly Queue<T> _available = new();
        private readonly HashSet<T> _active = new();

        public string PoolId { get; }
        public int ActiveCount => _active.Count;
        public int AvailableCount => _available.Count;

        public ObjectPool(string poolId, T prefab, Transform parent, int prewarmCount, int maxSize)
        {
            PoolId = poolId;
            _prefab = prefab;
            _parent = parent;
            _maxSize = maxSize;
            Prewarm(prewarmCount);
        }

        public void Prewarm(int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (_available.Count + _active.Count >= _maxSize)
                {
                    break;
                }

                var instance = CreateInstance();
                instance.gameObject.SetActive(false);
                _available.Enqueue(instance);
            }
        }

        public T Spawn(Vector3 position, Quaternion rotation)
        {
            T instance;
            if (_available.Count > 0)
            {
                instance = _available.Dequeue();
            }
            else if (_active.Count < _maxSize)
            {
                instance = CreateInstance();
            }
            else
            {
                Debug.LogWarning($"Pool '{PoolId}' reached max size {_maxSize}.");
                return null;
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);
            instance.OnSpawn();
            _active.Add(instance);
            return instance;
        }

        public void Despawn(T instance)
        {
            if (instance == null || !_active.Remove(instance))
            {
                return;
            }

            instance.OnDespawn();
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent, false);
            _available.Enqueue(instance);
        }

        private T CreateInstance()
        {
            var instance = Object.Instantiate(_prefab, _parent);
            return instance;
        }
    }
}
