using Studio.Core.Events;

namespace Studio.Core.SceneLoading
{
    public readonly struct SceneLoadStartedEvent : IEvent
    {
        public readonly string SceneName;

        public SceneLoadStartedEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }

    public readonly struct SceneLoadProgressEvent : IEvent
    {
        public readonly string SceneName;
        public readonly float Progress;

        public SceneLoadProgressEvent(string sceneName, float progress)
        {
            SceneName = sceneName;
            Progress = progress;
        }
    }

    public readonly struct SceneLoadCompletedEvent : IEvent
    {
        public readonly string SceneName;

        public SceneLoadCompletedEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
