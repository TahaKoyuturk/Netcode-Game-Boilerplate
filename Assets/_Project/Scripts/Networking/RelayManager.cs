using System.Threading.Tasks;
using Studio.Core.Services;
using NetworkConfig = Studio.Data.NetworkConfig;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class RelayManager : IService
    {
        private readonly NetworkConfig _config;

        public string JoinCode { get; private set; }

        public RelayManager(NetworkConfig config)
        {
            _config = config;
        }

        public async Task<string> AllocateHostRelayAsync()
        {
            var maxConnections = Mathf.Max(1, _config.MaxPlayers - 1);
            var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, _config.RelayRegion);
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            ApplyHostAllocation(allocation);
            return JoinCode;
        }

        public async Task JoinRelayAsync(string joinCode)
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            JoinCode = joinCode;
            ApplyClientAllocation(allocation);
        }

        private static void ApplyHostAllocation(Allocation allocation)
        {
            var transport = GetTransport();
            if (transport == null)
            {
                return;
            }

            transport.SetRelayServerData(allocation.ToRelayServerData("dtls"));
        }

        private static void ApplyClientAllocation(JoinAllocation allocation)
        {
            var transport = GetTransport();
            if (transport == null)
            {
                return;
            }

            transport.SetRelayServerData(allocation.ToRelayServerData("dtls"));
        }

        private static UnityTransport GetTransport()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager.Singleton is null.");
                return null;
            }

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (transport == null)
            {
                Debug.LogError("UnityTransport component is missing on NetworkManager.");
            }

            return transport;
        }
    }
}
