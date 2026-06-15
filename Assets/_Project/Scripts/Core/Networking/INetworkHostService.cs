using System.Threading.Tasks;
using Studio.Core.Services;

namespace Studio.Core.Networking
{
    public interface INetworkHostService : IService
    {
        Task<bool> HostGameAsync(string lobbyName);
    }
}
