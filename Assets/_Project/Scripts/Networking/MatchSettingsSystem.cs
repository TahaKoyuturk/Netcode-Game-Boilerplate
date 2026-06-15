using Unity.Collections;
using Unity.Netcode;

namespace Studio.Networking
{
    public struct MatchSettings : INetworkSerializable
    {
        public FixedString64Bytes MapId;
        public FixedString64Bytes ModeId;
        public int MaxPlayers;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref MapId);
            serializer.SerializeValue(ref ModeId);
            serializer.SerializeValue(ref MaxPlayers);
        }
    }

    public sealed class MatchSettingsSystem : NetworkBehaviour
    {
        public NetworkVariable<MatchSettings> Settings = new(
            default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        [ServerRpc(RequireOwnership = false)]
        public void SetSettingsServerRpc(MatchSettings settings)
        {
            if (!IsServer)
            {
                return;
            }

            Settings.Value = settings;
        }
    }
}
