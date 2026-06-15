using Unity.Netcode;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class SyncedTimer : NetworkBehaviour
    {
        public NetworkVariable<float> TimeRemaining = new(
            0f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        private bool _isRunning;

        [ServerRpc(RequireOwnership = false)]
        public void StartTimerServerRpc(float duration)
        {
            TimeRemaining.Value = duration;
            _isRunning = true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void StopTimerServerRpc()
        {
            _isRunning = false;
            TimeRemaining.Value = 0f;
        }

        private void Update()
        {
            if (!IsServer || !_isRunning)
            {
                return;
            }

            TimeRemaining.Value = Mathf.Max(0f, TimeRemaining.Value - Time.deltaTime);
            if (TimeRemaining.Value <= 0f)
            {
                _isRunning = false;
            }
        }
    }
}
