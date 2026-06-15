# Studio Unity 6 Boilerplate

## Purpose

Production-ready Unity 6 framework for reusable Steam and multiplayer projects.

## Responsibilities

- Core application lifecycle (bootstrap, state machine, services)
- Manager-based subsystems (audio, UI, economy, loading)
- Netcode + Relay + Lobby multiplayer foundation
- Save, settings, and input rebinding

## Dependencies

- Unity 6 (6000.0+)
- URP, Input System, TextMeshPro, Cinemachine
- Netcode for GameObjects, Unity Gaming Services (Core, Authentication, Multiplayer)
- ZString, NaughtyAttributes, ParrelSync (editor multiplayer clone testing)

## Usage Examples

1. Open project in Unity 6 and wait for package import.
2. Run **Studio > Setup > Initialize Project**.
3. Open **Bootstrap** scene and press Play.

## Common Workflows

- Add gameplay: create new `IState` and register in `Bootstrapper`.
- Add manager: implement `IManager`, register in `Bootstrapper.RegisterManagers()`.
- Host multiplayer: Main Menu Host button calls `NetworkBootstrap.HostGameAsync()`.

## Extension Guide

See `ARCHITECTURE.md` and subsystem docs in this folder.

## Best Practices

- Resolve dependencies via `ServiceLocator`, not singletons (except `GameManager`/`Bootstrapper`).
- Publish cross-system events through `EventBus`.
- Keep networking code in `Project.Networking` assembly.
