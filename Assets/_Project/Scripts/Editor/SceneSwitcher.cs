using UnityEditor;
using UnityEditor.SceneManagement;

namespace Studio.Editor
{
    public static class SceneSwitcher
    {
        private const string BootstrapPath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string MainMenuPath = "Assets/_Project/Scenes/MainMenu.unity";
        private const string GameplayPath = "Assets/_Project/Scenes/Gameplay.unity";

        [MenuItem("Studio/Scenes/Open Bootstrap")]
        public static void OpenBootstrap() => Open(BootstrapPath);

        [MenuItem("Studio/Scenes/Open Main Menu")]
        public static void OpenMainMenu() => Open(MainMenuPath);

        [MenuItem("Studio/Scenes/Open Gameplay")]
        public static void OpenGameplay() => Open(GameplayPath);

        private static void Open(string path)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path);
            }
        }
    }
}
