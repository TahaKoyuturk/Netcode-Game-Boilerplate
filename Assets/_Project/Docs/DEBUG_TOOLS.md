# Debug Tools

## Purpose

Development-only cheats, overlay menu, and command console.

## Responsibilities

- `DeveloperCheats`: `[Cheat]` attributed methods
- `RuntimeDebugMenu`: F1 IMGUI overlay
- `CommandConsole`: backtick command parser

## Dependencies

`GameConfig.EnableDebugTools`, DEVELOPMENT_BUILD or UNITY_EDITOR.

## Usage Examples

Console commands:
- `help`
- `state Menu`
- `currency 1000`
- `scene Gameplay`

## Common Workflows

- Press F1 for cheat buttons.
- Press ` (backtick) for command line.

## Extension Guide

```csharp
[Cheat("My Cheat")]
private void MyCheat() { }
```

## Best Practices

- Gate all debug code with `#if DEVELOPMENT_BUILD || UNITY_EDITOR`.
- Disable in production builds via `EnableDebugTools` and stripping.
