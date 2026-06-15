using System.Collections.Generic;
using System.Linq;
using Studio.Core;
using Studio.Gameplay;
using Studio.Managers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Studio.Editor
{
    public sealed class ProjectValidator : EditorWindow
    {
        private readonly List<ValidationResult> _results = new();
        private Vector2 _scroll;

        [MenuItem("Studio/Validate/Run Full Validation")]
        public static void ShowWindow()
        {
            var window = GetWindow<ProjectValidator>("Project Validator");
            window.RunValidation();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Run Validation"))
            {
                RunValidation();
            }

            _scroll = GUILayout.BeginScrollView(_scroll);
            for (var i = 0; i < _results.Count; i++)
            {
                var result = _results[i];
                var color = result.Passed ? Color.green : Color.red;
                var prev = GUI.color;
                GUI.color = color;
                GUILayout.Label($"{(result.Passed ? "PASS" : "FAIL")} - {result.CheckName}: {result.Message}");
                GUI.color = prev;
            }

            GUILayout.EndScrollView();
        }

        private void RunValidation()
        {
            _results.Clear();
            CheckMissingReferences();
            CheckMissingScripts();
            CheckDuplicateSingletons();
            CheckDuplicateManagers();
            CheckMissingEventSystem();
            CheckMissingNetworkManager();
            CheckBuildSettings();
            CheckBrokenPrefabs();
            CheckEmptyScenes();
            Repaint();
        }

        private void CheckMissingReferences()
        {
            var issues = MissingReferenceScanner.ScanProject();
            _results.Add(new ValidationResult("Missing References", issues.Count == 0,
                issues.Count == 0 ? "No missing references." : $"{issues.Count} missing references found."));
        }

        private void CheckMissingScripts()
        {
            var count = 0;
            var guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/_Project" });
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (obj == null) continue;
                var components = obj.GetComponentsInChildren<Component>(true);
                count += components.Count(c => c == null);
            }

            _results.Add(new ValidationResult("Missing Scripts", count == 0,
                count == 0 ? "No missing scripts." : $"{count} missing scripts found."));
        }

        private void CheckDuplicateSingletons()
        {
            var bootstrappers = Object.FindObjectsByType<Bootstrapper>(FindObjectsSortMode.None);
            var gameManagers = Object.FindObjectsByType<GameManager>(FindObjectsSortMode.None);
            var passed = bootstrappers.Length <= 1 && gameManagers.Length <= 1;
            _results.Add(new ValidationResult("Duplicate Singletons", passed,
                passed ? "Singleton counts are valid." : $"Bootstrapper={bootstrappers.Length}, GameManager={gameManagers.Length}"));
        }

        private void CheckDuplicateManagers()
        {
            var duplicateRoots = new[] { "UIRoot", "PoolRoot", "AudioManager" }
                .Select(name => new { name, count = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None).Count(go => go.name == name) })
                .Where(x => x.count > 1)
                .Select(x => x.name)
                .ToList();

            _results.Add(new ValidationResult("Duplicate Managers", duplicateRoots.Count == 0,
                duplicateRoots.Count == 0 ? "No duplicate manager roots in scene." : string.Join(", ", duplicateRoots)));
        }

        private void CheckMissingEventSystem()
        {
            var scenes = GetProjectScenes();
            var missing = new List<string>();
            for (var i = 0; i < scenes.Length; i++)
            {
                var scene = EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Single);
                var hasCanvas = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Length > 0;
                var hasEventSystem = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length > 0;
                if (hasCanvas && !hasEventSystem)
                {
                    missing.Add(scene.path);
                }
            }

            _results.Add(new ValidationResult("Missing EventSystem", missing.Count == 0,
                missing.Count == 0 ? "All UI scenes have EventSystem." : string.Join(", ", missing)));
        }

        private void CheckMissingNetworkManager()
        {
            const string gameplay = "Assets/_Project/Scenes/Gameplay.unity";
            var scene = EditorSceneManager.OpenScene(gameplay, OpenSceneMode.Single);
            var hasNetworkManager = Object.FindObjectsByType<NetworkManager>(FindObjectsSortMode.None).Length > 0;
            _results.Add(new ValidationResult("Missing NetworkManager", hasNetworkManager,
                hasNetworkManager ? "Gameplay scene has NetworkManager." : "Gameplay scene is missing NetworkManager."));
        }

        private void CheckBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToList();
            var passed = scenes.Count >= 3 &&
                         scenes[0].EndsWith("Bootstrap.unity") &&
                         scenes.Any(s => s.EndsWith("MainMenu.unity")) &&
                         scenes.Any(s => s.EndsWith("Gameplay.unity"));
            _results.Add(new ValidationResult("Invalid Build Settings", passed,
                passed ? "Build settings are configured." : "Bootstrap must be index 0 with all project scenes included."));
        }

        private void CheckBrokenPrefabs()
        {
            var broken = 0;
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Project/Prefabs" });
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    broken++;
                    continue;
                }

                if (prefab.GetComponentsInChildren<Component>(true).Any(c => c == null))
                {
                    broken++;
                }
            }

            _results.Add(new ValidationResult("Broken Prefabs", broken == 0,
                broken == 0 ? "No broken prefabs." : $"{broken} broken prefabs found."));
        }

        private void CheckEmptyScenes()
        {
            var empty = new List<string>();
            var scenes = GetProjectScenes();
            for (var i = 0; i < scenes.Length; i++)
            {
                var scene = EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Single);
                var roots = scene.GetRootGameObjects();
                var meaningful = roots.Count(go =>
                    go.GetComponent<Camera>() == null &&
                    go.name != "Directional Light" &&
                    go.name != "Global Volume");
                if (meaningful == 0 && roots.Length <= 2)
                {
                    empty.Add(scene.path);
                }
            }

            _results.Add(new ValidationResult("Empty Scenes", empty.Count == 0,
                empty.Count == 0 ? "No empty scenes." : string.Join(", ", empty)));
        }

        private static string[] GetProjectScenes()
        {
            return new[]
            {
                "Assets/_Project/Scenes/Bootstrap.unity",
                "Assets/_Project/Scenes/MainMenu.unity",
                "Assets/_Project/Scenes/Gameplay.unity"
            };
        }

        private readonly struct ValidationResult
        {
            public string CheckName { get; }
            public bool Passed { get; }
            public string Message { get; }

            public ValidationResult(string checkName, bool passed, string message)
            {
                CheckName = checkName;
                Passed = passed;
                Message = message;
            }
        }
    }
}
