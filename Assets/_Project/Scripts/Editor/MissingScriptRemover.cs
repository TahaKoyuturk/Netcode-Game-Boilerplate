using UnityEditor;
using UnityEngine;

namespace Studio.Editor
{
    public static class MissingScriptRemover
    {
        public static int RemoveMissingScripts(GameObject root)
        {
            var count = 0;
            var transforms = root.GetComponentsInChildren<Transform>(true);
            for (var i = 0; i < transforms.Length; i++)
            {
                count += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(transforms[i].gameObject);
            }

            return count;
        }

        [MenuItem("Studio/Tools/Remove Missing Scripts")]
        public static void RemoveFromSelection()
        {
            var total = 0;
            foreach (var obj in Selection.gameObjects)
            {
                total += RemoveMissingScripts(obj);
            }

            UnityEngine.Debug.Log($"Removed {total} missing scripts.");
        }
    }
}
