using Studio.Systems.Pooling;
using Unity.Netcode;

namespace Studio.Networking
{
    public class PooledNetworkBehaviour : NetworkBehaviour, IPoolable
    {
        public virtual void OnSpawn()
        {
        }

        public virtual void OnDespawn()
        {
        }
    }
}
