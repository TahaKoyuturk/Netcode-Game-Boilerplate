using UnityEngine;

namespace Studio.Systems.Pooling
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Studio/Pooling/Pool Config")]
    public sealed class PoolConfig : ScriptableObject
    {
        public string PoolId;
        public Component Prefab;
        public int PrewarmCount = 5;
        public int MaxSize = 50;
    }
}
