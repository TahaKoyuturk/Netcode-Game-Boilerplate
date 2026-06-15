# Settings System

## Purpose

Runtime-adjustable game settings with apply + persist pipeline.

## Responsibilities

- `SettingsSystem`: owns `Setting<T>` values and registry
- `SettingsApplier`: resolution, quality, fullscreen, vsync
- `SettingsManager`: facade for UI and auto-save

## Dependencies

`SettingsConfig` ScriptableObject, `ISaveService`.

## Usage Examples

```csharp
var settings = ServiceLocator.Get<SettingsManager>();
settings.SetQuality(3);
settings.SetMasterVolume(0.75f);
```

## Common Workflows

1. UI slider changes value on `SettingsSystem.MasterVolume`.
2. Call `SettingsSystem.ApplyAll()` and `Save()`.

## Extension Guide

```csharp
var fov = new Setting<float>("fov", 60f);
settingsSystem.Registry.Register(fov);
```

## Best Practices

- Apply audio volumes through `AudioManager.ApplyVolumes()` after changes.
- Defaults live in `SettingsConfig`, overrides in `SettingsSaveData`.
