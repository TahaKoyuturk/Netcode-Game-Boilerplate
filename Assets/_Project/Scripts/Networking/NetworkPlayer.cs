using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class NetworkPlayer : NetworkBehaviour
    {
        public NetworkVariable<FixedString64Bytes> PlayerName = new(
            default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> IsReady = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                PlayerName.Value = $"Player_{OwnerClientId}";
            }
        }

        [ServerRpc]
        public void SetReadyServerRpc(bool ready)
        {
            IsReady.Value = ready;
        }

        public void SetReady(bool ready)
        {
            if (!IsOwner)
            {
                return;
            }

            IsReady.Value = ready;
            SetReadyServerRpc(ready);
        }
    }
}
