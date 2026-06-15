# Networking

## Purpose

Host/client multiplayer via Unity Lobby, Relay, and Netcode for GameObjects.

## Responsibilities

- **NetworkBootstrap**: UGS init, anonymous auth, host/join API
- **LobbyManager**: create/join/leave/query lobbies
- **RelayManager**: allocate/join relay, configure `UnityTransport`
- **NetworkManagerWrapper**: start host/client, connection events
- **NetworkSceneLoader**: server-driven scene sync
- **ReadyCheckSystem**: all-ready detection
- **MatchSettingsSystem**: synced match config
- **SyncedTimer**: server-authoritative countdown
- **ConnectionWatchdog**: timeout detection

## Dependencies

UGS project link, `NetworkConfig` ScriptableObject, NetworkManager in Gameplay scene.

## Usage Examples

```csharp
var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
await bootstrap.HostGameAsync("MyLobby");
// or
await bootstrap.JoinGameAsync(lobbyId);
```

## Common Workflows

1. Host: Auth → Relay allocation → Lobby create → `StartHost`
2. Client: Auth → Join lobby → Relay join → `StartClient`
3. Ready: `ReadyCheckSystem.SetLocalReady(true)`
4. Start match: host calls `NetworkSceneLoader.LoadGameplaySceneAsync()`

## Extension Guide

- Add `NetworkBehaviour` gameplay scripts in `Project.Networking` or game assembly.
- Store lobby metadata in `Lobby.Data` dictionary.

## Best Practices

- Server authority for gameplay-altering RPCs.
- Keep transport configuration in `RelayManager` only.
