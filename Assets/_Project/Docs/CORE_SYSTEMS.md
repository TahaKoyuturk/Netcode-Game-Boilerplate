# Core Systems

## Purpose

Foundation services used by all other layers.

## Responsibilities

- **ServiceLocator**: register/resolve `IService` implementations
- **EventBus**: typed pub/sub via `IEvent` structs
- **StateMachine**: `IState` lifecycle with push/pop/replace
- **TickSystem**: priority-sorted `ITickable` dispatch
- **SceneLoader**: async scene load with progress events
- **SaveService**: versioned JSON persistence
- **GameManager**: owns state machine and `GameContext`

## Dependencies

`Project.Core` only references Unity Engine and Input System.

## Usage Examples

```csharp
EventBus.Subscribe<CurrencyChangedEvent>(e => Debug.Log(e.Balance));
EventBus.Publish(new CurrencyChangedEvent(100));

gameManager.StateMachine.ChangeState(MenuState.StateId);
```

## Common Workflows

- Subscribe in `OnShow`, unsubscribe in `OnHide`.
- Save: `saveService.Save("key", new MySaveData());`

## Extension Guide

- New event: `public readonly struct MyEvent : IEvent { }`
- New state: implement `IState`, register in `Bootstrapper`.

## Best Practices

- Clear `ServiceLocator` only on domain reload (handled automatically).
- Keep `IState.OnTick` lightweight.
