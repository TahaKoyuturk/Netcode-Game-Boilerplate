using UnityEngine.SceneManagement;

namespace Studio.Core.SceneLoading
{
    public sealed class SceneLoadRequest
    {
        public string SceneName { get; set; }
        public LoadSceneMode LoadMode { get; set; } = LoadSceneMode.Single;
        public bool ActivateOnLoad { get; set; } = true;
    }
}
