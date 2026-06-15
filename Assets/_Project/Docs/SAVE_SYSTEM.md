# Save System

## Purpose

Versioned JSON persistence to `Application.persistentDataPath`.

## Responsibilities

- `ISaveService`: Save, TryLoad, Delete, Exists
- `SaveDataBase`: version field + `Migrate()` hook
- Concrete saves: Settings, Currency, InputBindings

## Dependencies

`Project.Core` — no Unity scene references.

## Usage Examples

```csharp
var save = ServiceLocator.Get<ISaveService>();
save.Save("profile", new CurrencySaveData { Balance = 500 });
if (save.TryLoad("profile", out CurrencySaveData data)) { /* use data */ }
```

## Common Workflows

- Bump `Version` in save class and override `Migrate()` for schema changes.
- Use unique keys per feature (`CurrencyManager.SaveKey`).

## Extension Guide

```csharp
[Serializable]
public sealed class ProfileSaveData : SaveDataBase {
    public string PlayerName;
    public override void Migrate() { if (Version < 2) { /* upgrade */ Version = 2; } }
}
```

## Best Practices

- Keep save DTOs in `Studio.Data.Save`.
- Never store secrets in JSON saves.
