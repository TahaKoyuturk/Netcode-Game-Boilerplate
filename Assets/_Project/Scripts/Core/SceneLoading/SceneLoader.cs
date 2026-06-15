using System.Threading.Tasks;
using Studio.Core.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Studio.Core.SceneLoading
{
    public sealed class SceneLoader : ISceneLoader
    {
        public bool IsLoading { get; private set; }

        public async Task LoadAsync(SceneLoadRequest request)
        {
            if (IsLoading)
            {
                Debug.LogWarning("SceneLoader is already loading a scene.");
                return;
            }

            IsLoading = true;
            EventBus.Publish(new SceneLoadStartedEvent(request.SceneName));

            var operation = SceneManager.LoadSceneAsync(request.SceneName, request.LoadMode);
            if (operation == null)
            {
                Debug.LogError($"Failed to load scene '{request.SceneName}'.");
                IsLoading = false;
                return;
            }

            operation.allowSceneActivation = request.ActivateOnLoad;

            while (!operation.isDone)
            {
                EventBus.Publish(new SceneLoadProgressEvent(request.SceneName, operation.progress));
                await Task.Yield();
            }

            EventBus.Publish(new SceneLoadCompletedEvent(request.SceneName));
            IsLoading = false;
        }
    }
}
