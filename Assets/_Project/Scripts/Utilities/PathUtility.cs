using System.IO;
using UnityEngine;

namespace Studio.Utilities
{
    public static class PathUtility
    {
        public static string GetPersistentFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
