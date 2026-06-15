# Input Rebinding

## Purpose

Runtime rebinding with persistence using Unity Input System.

## Responsibilities

- `GameInputActions`: wrapper over `InputActionAsset`
- `InputBindingService`: interactive rebind, save/load/reset
- `RebindUIController`: UI hookup base class

## Dependencies

`Assets/_Project/Settings/GameInputActions.inputactions`, Input System package.

## Usage Examples

```csharp
var input = ServiceLocator.Get<InputBindingService>();
input.StartRebind(action, bindingIndex, success => { });
input.ResetBindings();
```

## Common Workflows

1. Assign `InputActionReference` on `RebindUIController`.
2. Player clicks Rebind → interactive rebind → auto-save.

## Extension Guide

Add actions to `GameInputActions.inputactions`, regenerate if using codegen, or load asset directly.

## Best Practices

- Disable gameplay actions during rebind UI.
- Store overrides via `SaveBindingOverridesAsJson`.
