using UnityEngine;

namespace Studio.Data
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Studio/Config/Network Config")]
    public sealed class NetworkConfig : ScriptableObject
    {
        public int MaxPlayers = 4;
        public string RelayRegion = "us-west";
        public float LobbyHeartbeatSeconds = 15f;
        public float ConnectionTimeoutSeconds = 30f;
        public float ReadyCheckTimeoutSeconds = 60f;
        public string DefaultGameplayScene = "Gameplay";
    }
}
