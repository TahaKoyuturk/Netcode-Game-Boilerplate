namespace Studio.Core.Tick
{
    public interface ITickable
    {
        int TickPriority { get; }
        void OnTick(float deltaTime);
    }

    public interface IFixedTickable
    {
        int FixedTickPriority { get; }
        void OnFixedTick(float fixedDeltaTime);
    }
}
