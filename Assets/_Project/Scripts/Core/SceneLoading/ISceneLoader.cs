using System.Threading.Tasks;
using Studio.Core.Services;

namespace Studio.Core.SceneLoading
{
    public interface ISceneLoader : IService
    {
        bool IsLoading { get; }
        Task LoadAsync(SceneLoadRequest request);
    }
}
