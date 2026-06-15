using Studio.Core.Services;

namespace Studio.Core.Save
{
    public interface ISaveService : IService
    {
        bool Exists(string key);
        void Save<T>(string key, T data) where T : SaveDataBase;
        bool TryLoad<T>(string key, out T data) where T : SaveDataBase, new();
        void Delete(string key);
    }
}
