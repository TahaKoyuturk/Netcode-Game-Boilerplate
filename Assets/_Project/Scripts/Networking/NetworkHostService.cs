using System.Threading.Tasks;
using Studio.Core.Networking;

namespace Studio.Networking
{
    public sealed class NetworkHostService : INetworkHostService
    {
        private readonly LobbyManager _lobbyManager;
        private readonly NetworkManagerWrapper _networkWrapper;

        public NetworkHostService(LobbyManager lobbyManager, NetworkManagerWrapper networkWrapper)
        {
            _lobbyManager = lobbyManager;
            _networkWrapper = networkWrapper;
        }

        public async Task<bool> HostGameAsync(string lobbyName)
        {
            await _lobbyManager.CreateLobbyAsync(lobbyName);
            return _networkWrapper.StartHost();
        }
    }
}
