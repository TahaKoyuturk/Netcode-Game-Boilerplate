using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Studio.Editor
{
    public static class MissingReferenceScanner
    {
        public static List<string> ScanProject()
        {
            var issues = new List<string>();
            var guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/_Project" });

            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (obj == null)
                {
                    continue;
                }

                var components = obj.GetComponentsInChildren<Component>(true);
                for (var c = 0; c < components.Length; c++)
                {
                    if (components[c] == null)
                    {
                        issues.Add($"Missing script on '{path}'");
                        continue;
                    }

                    var so = new SerializedObject(components[c]);
                    var prop = so.GetIterator();
                    while (prop.NextVisible(true))
                    {
                        if (prop.propertyType != SerializedPropertyType.ObjectReference)
                        {
                            continue;
                        }

                        if (prop.objectReferenceValue == null && prop.objectReferenceInstanceIDValue != 0)
                        {
                            issues.Add($"Missing reference '{prop.name}' on {components[c].GetType().Name} in '{path}'");
                        }
                    }
                }
            }

            return issues;
        }

        [MenuItem("Studio/Validate/Missing References")]
        public static void RunFromMenu()
        {
            var issues = ScanProject();
            if (issues.Count == 0)
            {
                UnityEngine.Debug.Log("MissingReferenceScanner: No issues found.");
                return;
            }

            for (var i = 0; i < issues.Count; i++)
            {
                UnityEngine.Debug.LogWarning(issues[i]);
            }
        }
    }
}
