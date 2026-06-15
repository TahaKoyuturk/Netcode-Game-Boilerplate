using System;
using System.IO;
using UnityEngine;

namespace Studio.Core.Save
{
    public sealed class SaveService : ISaveService
    {
        private readonly string _rootPath;

        public SaveService()
        {
            _rootPath = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        public bool Exists(string key)
        {
            return File.Exists(GetFilePath(key));
        }

        public void Save<T>(string key, T data) where T : SaveDataBase
        {
            var wrapper = new SaveWrapper<T>
            {
                Metadata = new SaveMetadata
                {
                    Key = key,
                    Version = data.Version,
                    SavedAtUtc = DateTime.UtcNow.ToString("O")
                },
                Data = data
            };

            var json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(GetFilePath(key), json);
        }

        public bool TryLoad<T>(string key, out T data) where T : SaveDataBase, new()
        {
            data = null;
            var path = GetFilePath(key);
            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                var json = File.ReadAllText(path);
                var wrapper = JsonUtility.FromJson<SaveWrapper<T>>(json);
                if (wrapper?.Data == null)
                {
                    return false;
                }

                wrapper.Data.Migrate();
                data = wrapper.Data;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load save '{key}': {ex.Message}");
                return false;
            }
        }

        public void Delete(string key)
        {
            var path = GetFilePath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(_rootPath, $"{key}.json");
        }

        [Serializable]
        private sealed class SaveWrapper<T> where T : SaveDataBase
        {
            public SaveMetadata Metadata;
            public T Data;
        }
    }
}
