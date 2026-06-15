using Studio.Core.Services;

namespace Studio.Managers
{
    public interface IManager : IService
    {
        void Initialize();
        void Shutdown();
    }
}
