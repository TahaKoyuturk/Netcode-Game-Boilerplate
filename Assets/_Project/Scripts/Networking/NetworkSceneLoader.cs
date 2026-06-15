using System.Threading.Tasks;
using NetworkConfig = Studio.Data.NetworkConfig;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Studio.Networking
{
    public sealed class NetworkSceneLoader : NetworkBehaviour
    {
        [SerializeField] private NetworkConfig networkConfig;

        public async Task LoadGameplaySceneAsync()
        {
            if (!IsServer)
            {
                return;
            }

            var sceneName = networkConfig != null
                ? networkConfig.DefaultGameplayScene
                : "Gameplay";

            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogError($"Failed to start networked scene load: {status}");
            }

            await Task.CompletedTask;
        }
    }
}
