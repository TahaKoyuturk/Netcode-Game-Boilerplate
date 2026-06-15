# Managers

## Purpose

Application-level facades with single responsibilities.

## Responsibilities

| Manager | Role |
|---------|------|
| UIManager | Panel stack and registration |
| AudioManager | Music/SFX with settings volumes |
| PoolManager | Registry of `ObjectPool<T>` |
| CurrencyManager | Balance with persistence |
| SettingsManager | Applies and saves settings |
| NotificationManager | Toast queue |
| LoadingManager | Loading pipeline + scene loads |
| PopupManager | Modal popup queue |

## Dependencies

`Project.Managers` → Core, Systems, Data, Pooling.

## Usage Examples

```csharp
ServiceLocator.Get<NotificationManager>().Show(new NotificationData {
    Title = "Welcome", Message = "Game started"
});
await ServiceLocator.Get<LoadingManager>().LoadSceneAsync("Gameplay");
```

## Common Workflows

- Show loading: `LoadingManager.LoadSceneAsync` publishes progress events consumed by `LoadingScreen`.
- Modal confirm: `PopupManager.Show(new PopupRequest { PopupId = "ConfirmPopup", ... })`.

## Extension Guide

Implement `IManager`, register in `Bootstrapper.RegisterManagers()`, call `Initialize()`.

## Best Practices

- Managers expose behavior, not UI layout.
- Persist in `Shutdown()` when needed.
