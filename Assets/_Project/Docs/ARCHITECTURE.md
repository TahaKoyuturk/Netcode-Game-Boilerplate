# Architecture

## Purpose

Define layered, decoupled structure for maintainable multi-project reuse.

## Responsibilities

| Layer | Assembly | Role |
|-------|----------|------|
| Core | Project.Core | ServiceLocator, EventBus, StateMachine, Save, SceneLoader |
| Data | Project.Data | ScriptableObject configs and save DTOs |
| Systems | Project.Systems | Settings, Input, Loading, Notification, Popup, Tween |
| Managers | Project.Managers | Facades orchestrating systems |
| UI | Project.UI | Panels, windows, popups |
| Pooling | Project.Pooling | Generic object pools |
| Networking | Project.Networking | UGS, Lobby, Relay, Netcode |
| Gameplay | Project.Gameplay | Bootstrapper, game states |
| Debug | Project.Debug | Cheats, console, debug menu |
| Editor | Project.Editor | Validation and setup tools |

## Dependencies

Inner layers never reference UI or Networking. Managers never reference Networking.

## Usage Examples

```csharp
ServiceLocator.Register(new CurrencyManager(save, economyConfig));
var currency = ServiceLocator.Get<CurrencyManager>();
currency.Earn(50);
```

## Common Workflows

1. Bootstrap scene registers services.
2. `GameManager` state machine drives Menu → Lobby → Gameplay.
3. Events propagate UI updates without direct references.

## Extension Guide

- New assembly: add `.asmdef`, reference only lower layers.
- New config: create ScriptableObject in `Studio.Data`, wire in `GameConfig`.

## Best Practices

- Prefer composition and interfaces over inheritance.
- No circular assembly references.
- Use namespaces `Studio.<Area>.<Feature>`.
