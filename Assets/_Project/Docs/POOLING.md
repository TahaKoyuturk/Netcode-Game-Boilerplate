# Pooling

## Purpose

Reduce instantiate/destroy overhead for frequent spawn objects.

## Responsibilities

- `IPoolable`: `OnSpawn`/`OnDespawn` hooks
- `ObjectPool<T>`: prewarm, spawn, despawn with max size
- `PoolConfig`: ScriptableObject pool definition
- `PoolManager`: pool registry
- `NetworkObjectPool`: server-side pooled `NetworkObject` spawn

## Dependencies

`Project.Pooling`, prefab implementing `IPoolable`.

## Usage Examples

```csharp
var pool = ServiceLocator.Get<PoolManager>().CreatePool<MyPoolable>(poolConfig);
var obj = pool.Spawn(position, rotation);
pool.Despawn(obj);
```

## Common Workflows

1. Create `PoolConfig` asset with prefab and prewarm count.
2. `PoolManager.CreatePool` at startup.
3. Spawn/despawn during gameplay.

## Extension Guide

For networked pools, subclass `PooledNetworkBehaviour` and use `NetworkObjectPool`.

## Best Practices

- Reset state in `OnSpawn`/`OnDespawn`.
- Set realistic `MaxSize` to cap memory.
