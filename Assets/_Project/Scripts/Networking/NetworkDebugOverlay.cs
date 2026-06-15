using Unity.Netcode;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class NetworkDebugOverlay : MonoBehaviour
    {
        private void OnGUI()
        {
#if !DEVELOPMENT_BUILD && !UNITY_EDITOR
            return;
#endif
            if (NetworkManager.Singleton == null)
            {
                GUI.Label(new Rect(10, 10, 400, 20), "Network: No NetworkManager");
                return;
            }

            var nm = NetworkManager.Singleton;
            GUI.Label(new Rect(10, 10, 500, 20), $"Role: Host={nm.IsHost} Client={nm.IsClient} Server={nm.IsServer}");
            GUI.Label(new Rect(10, 30, 500, 20), $"Connected Clients: {nm.ConnectedClientsIds.Count}");
            GUI.Label(new Rect(10, 50, 500, 20), $"Local Client ID: {nm.LocalClientId}");
            GUI.Label(new Rect(10, 70, 500, 20), $"IsConnectedClient: {nm.IsConnectedClient}");
        }
    }
}
