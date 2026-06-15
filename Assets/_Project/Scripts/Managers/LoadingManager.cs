using System.Threading.Tasks;
using Studio.Core.SceneLoading;
using Studio.Core.Services;
using Studio.Systems.Loading;

namespace Studio.Managers
{
    public sealed class LoadingManager : IManager
    {
        private readonly LoadingPipeline _pipeline;
        private readonly ISceneLoader _sceneLoader;

        public LoadingManager(LoadingPipeline pipeline, ISceneLoader sceneLoader)
        {
            _pipeline = pipeline;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
        }

        public void Shutdown()
        {
        }

        public Task LoadSceneAsync(string sceneName, bool additive = false)
        {
            _pipeline.Enqueue(new LoadingOperation($"load_{sceneName}", async progress =>
            {
                progress.Report(0f);
                await _sceneLoader.LoadAsync(new SceneLoadRequest
                {
                    SceneName = sceneName,
                    LoadMode = additive
                        ? UnityEngine.SceneManagement.LoadSceneMode.Additive
                        : UnityEngine.SceneManagement.LoadSceneMode.Single
                });
                progress.Report(1f);
            }));

            return _pipeline.RunAsync();
        }
    }
}
