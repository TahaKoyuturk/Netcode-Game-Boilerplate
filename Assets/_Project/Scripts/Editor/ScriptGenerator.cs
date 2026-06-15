using System.IO;
using UnityEditor;
using UnityEngine;

namespace Studio.Editor
{
    public static class ScriptGenerator
    {
        private const string Template = @"using UnityEngine;

namespace Studio.{0}
{{
    public sealed class {1} : MonoBehaviour
    {{
    }}
}}
";

        [MenuItem("Assets/Create/Studio/C# Script", false, 0)]
        public static void CreateScript()
        {
            var name = "NewStudioScript";
            var path = EditorUtility.SaveFilePanelInProject("Create Studio Script", name, "cs", "Choose location");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var className = Path.GetFileNameWithoutExtension(path);
            var folderNamespace = GetNamespaceFromPath(path);
            var content = string.Format(Template, folderNamespace, className);
            File.WriteAllText(path, content);
            AssetDatabase.Refresh();
        }

        private static string GetNamespaceFromPath(string path)
        {
            if (path.Contains("/Core/")) return "Core";
            if (path.Contains("/Managers/")) return "Managers";
            if (path.Contains("/Networking/")) return "Networking";
            if (path.Contains("/UI/")) return "UI";
            if (path.Contains("/Gameplay/")) return "Gameplay";
            if (path.Contains("/Systems/")) return "Systems";
            if (path.Contains("/Debug/")) return "Debug";
            return "Gameplay";
        }
    }
}
