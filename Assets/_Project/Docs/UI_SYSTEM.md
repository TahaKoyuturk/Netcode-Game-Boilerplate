# UI System

## Purpose

Consistent panel/window/popup lifecycle with fade support.

## Responsibilities

- `UIPanelBase`: show/hide with `CanvasGroup` + `ITweenService`
- `UIWindowBase`: close button and Cancel action
- `PopupBase`: modal dialogs driven by `PopupManager`
- `LoadingScreen`: binds to loading progress events
- Panel implementations: MainMenu, Lobby, Pause, GameOver

## Dependencies

Project.UI → Managers, Systems, TextMeshPro, UGUI.

## Usage Examples

```csharp
// Register in panel Start()
ServiceLocator.Get<UIManager>().RegisterPanel(this);
ServiceLocator.Get<UIManager>().ShowPanel("MainMenu");
```

## Common Workflows

1. Create panel prefab with `CanvasGroup`.
2. Subclass `UIPanelBase`, set `panelId`.
3. Register on `Start`, show via `UIManager` or state entry.

## Extension Guide

Subclass `PopupBase`, assign `popupId`, place under `UIRoot`, call via `PopupManager.Show`.

## Best Practices

- One `EventSystem` per scene with UI.
- Use `RebindUIController` for input rebind screens.
