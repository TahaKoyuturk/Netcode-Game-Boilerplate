namespace Studio.Core.StateMachine
{
    public interface IState
    {
        string StateId { get; }
        void OnEnter();
        void OnExit();
        void OnTick(float deltaTime);
    }
}
