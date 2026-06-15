using Studio.Data;
using Studio.Gameplay;
using Studio.Networking;
using Studio.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using NetworkConfig = Studio.Data.NetworkConfig;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Studio.Editor
{
    public static class ProjectSetup
    {
        private const string ConfigPath = "Assets/_Project/ScriptableObjects";
        private const string ScenesPath = "Assets/_Project/Scenes";
        private const string InputAssetPath = "Assets/_Project/Settings/GameInputActions.inputactions";

        [MenuItem("Studio/Setup/Initialize Project")]
        public static void InitializeProject()
        {
            CreateConfigs();
            CreateScenes();
            UpdateBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("Studio boilerplate setup complete.");
        }

        private static void CreateConfigs()
        {
            EnsureFolder(ConfigPath);
            CreateIfMissing<AudioConfig>($"{ConfigPath}/AudioConfig.asset");
            CreateIfMissing<NetworkConfig>($"{ConfigPath}/NetworkConfig.asset");
            CreateIfMissing<EconomyConfig>($"{ConfigPath}/EconomyConfig.asset");
            CreateIfMissing<SettingsConfig>($"{ConfigPath}/SettingsConfig.asset");

            var gameConfig = CreateIfMissing<GameConfig>($"{ConfigPath}/GameConfig.asset");
            var so = new SerializedObject(gameConfig);
            so.FindProperty("AudioConfig").objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioConfig>($"{ConfigPath}/AudioConfig.asset");
            so.FindProperty("NetworkConfig").objectReferenceValue = AssetDatabase.LoadAssetAtPath<NetworkConfig>($"{ConfigPath}/NetworkConfig.asset");
            so.FindProperty("EconomyConfig").objectReferenceValue = AssetDatabase.LoadAssetAtPath<EconomyConfig>($"{ConfigPath}/EconomyConfig.asset");
            so.FindProperty("SettingsConfig").objectReferenceValue = AssetDatabase.LoadAssetAtPath<SettingsConfig>($"{ConfigPath}/SettingsConfig.asset");
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static T CreateIfMissing<T>(string path) where T : ScriptableObject
        {
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null)
            {
                return existing;
            }

            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void CreateScenes()
        {
            EnsureFolder(ScenesPath);
            CreateBootstrapScene($"{ScenesPath}/Bootstrap.unity");
            CreateMainMenuScene($"{ScenesPath}/MainMenu.unity");
            CreateGameplayScene($"{ScenesPath}/Gameplay.unity");
        }

        private static void CreateBootstrapScene(string path)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var root = new GameObject("Bootstrap");
            var bootstrapper = root.AddComponent<Bootstrapper>();
            root.AddComponent<Studio.Core.GameManager>();

            var gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>($"{ConfigPath}/GameConfig.asset");
            var inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            var so = new SerializedObject(bootstrapper);
            so.FindProperty("gameConfig").objectReferenceValue = gameConfig;
            so.FindProperty("inputActionsAsset").objectReferenceValue = inputAsset;
            so.ApplyModifiedPropertiesWithoutUndo();

            if (gameConfig != null && gameConfig.EnableDebugTools)
            {
                root.AddComponent<Studio.Debug.DeveloperCheats>();
                root.AddComponent<Studio.Debug.RuntimeDebugMenu>();
                root.AddComponent<Studio.Debug.CommandConsole>();
            }

            EditorSceneManager.SaveScene(scene, path);
        }

        private static void CreateMainMenuScene(string path)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            var uiRoot = new GameObject("UIRoot");
            var canvas = uiRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiRoot.AddComponent<CanvasScaler>();
            uiRoot.AddComponent<GraphicRaycaster>();

            CreatePanel<MainMenuPanel>(uiRoot.transform, "MainMenuPanel");
            CreatePanel<LobbyPanel>(uiRoot.transform, "LobbyPanel");
            CreatePanel<PausePanel>(uiRoot.transform, "PausePanel");
            CreatePanel<GameOverPanel>(uiRoot.transform, "GameOverPanel");
            CreatePanel<LoadingScreen>(uiRoot.transform, "LoadingScreen");
            CreatePanel<NotificationToastView>(uiRoot.transform, "NotificationToast");
            CreatePanel<ConfirmPopup>(uiRoot.transform, "ConfirmPopup");
            CreatePanel<AlertPopup>(uiRoot.transform, "AlertPopup");

            EditorSceneManager.SaveScene(scene, path);
        }

        private static void CreateGameplayScene(string path)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var networkGo = new GameObject("NetworkManager");
            networkGo.AddComponent<NetworkManager>();
            networkGo.AddComponent<UnityTransport>();
            networkGo.AddComponent<NetworkBootstrap>();
            networkGo.AddComponent<ConnectionWatchdog>();
            networkGo.AddComponent<NetworkSceneLoader>();
            networkGo.AddComponent<ReadyCheckSystem>();
            networkGo.AddComponent<MatchSettingsSystem>();
            networkGo.AddComponent<SyncedTimer>();
            networkGo.AddComponent<NetworkDebugOverlay>();

            new GameObject("SpawnPoint").transform.position = Vector3.zero;
            EditorSceneManager.SaveScene(scene, path);
        }

        private static GameObject CreatePanel<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasGroup));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            go.AddComponent<T>();
            return go;
        }

        private static void UpdateBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene($"{ScenesPath}/Bootstrap.unity", true),
                new EditorBuildSettingsScene($"{ScenesPath}/MainMenu.unity", true),
                new EditorBuildSettingsScene($"{ScenesPath}/Gameplay.unity", true)
            };
        }

        private static void EnsureFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                var parts = path.Split('/');
                var current = parts[0];
                for (var i = 1; i < parts.Length; i++)
                {
                    var next = current + "/" + parts[i];
                    if (!AssetDatabase.IsValidFolder(next))
                    {
                        AssetDatabase.CreateFolder(current, parts[i]);
                    }

                    current = next;
                }
            }
        }
    }
}
