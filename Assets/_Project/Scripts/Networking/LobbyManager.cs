using System.Collections.Generic;
using System.Threading.Tasks;
using Studio.Core.Services;
using NetworkConfig = Studio.Data.NetworkConfig;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Studio.Networking
{
    public sealed class LobbyManager : IService
    {
        private readonly NetworkConfig _config;
        private readonly RelayManager _relayManager;
        private ILobbyEvents _lobbyEvents;

        public Lobby CurrentLobby { get; private set; }
        public bool IsHost => CurrentLobby != null &&
                              CurrentLobby.HostId == AuthenticationService.Instance.PlayerId;

        public LobbyManager(NetworkConfig config, RelayManager relayManager)
        {
            _config = config;
            _relayManager = relayManager;
        }

        public async Task<Lobby> CreateLobbyAsync(string lobbyName, bool isPrivate = false)
        {
            var joinCode = await _relayManager.AllocateHostRelayAsync();
            var options = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            };

            CurrentLobby = await LobbyService.Instance.CreateLobbyAsync(
                lobbyName,
                _config.MaxPlayers,
                options);

            await SubscribeToLobbyEvents();
            return CurrentLobby;
        }

        public async Task<Lobby> JoinLobbyAsync(string lobbyId)
        {
            CurrentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            await SubscribeToLobbyEvents();

            if (CurrentLobby.Data != null &&
                CurrentLobby.Data.TryGetValue("RelayJoinCode", out var relayData))
            {
                await _relayManager.JoinRelayAsync(relayData.Value);
            }

            return CurrentLobby;
        }

        public async Task<IReadOnlyList<Lobby>> QueryLobbiesAsync()
        {
            var response = await LobbyService.Instance.QueryLobbiesAsync();
            return response.Results;
        }

        public async Task LeaveLobbyAsync()
        {
            if (CurrentLobby == null)
            {
                return;
            }

            if (_lobbyEvents != null)
            {
                await _lobbyEvents.UnsubscribeAsync();
                _lobbyEvents = null;
            }

            var lobbyId = CurrentLobby.Id;
            var wasHost = IsHost;
            CurrentLobby = null;

            if (wasHost)
            {
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            }
            else
            {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
            }
        }

        public async Task SendHeartbeatAsync()
        {
            if (CurrentLobby == null || !IsHost)
            {
                return;
            }

            await LobbyService.Instance.SendHeartbeatPingAsync(CurrentLobby.Id);
        }

        private async Task SubscribeToLobbyEvents()
        {
            if (CurrentLobby == null)
            {
                return;
            }

            var callbacks = new LobbyEventCallbacks();
            callbacks.LobbyChanged += changes =>
            {
                if (changes.LobbyDeleted)
                {
                    CurrentLobby = null;
                }
            };

            _lobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(CurrentLobby.Id, callbacks);
        }
    }
}
