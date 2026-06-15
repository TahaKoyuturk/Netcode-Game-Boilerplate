using System.Collections.Generic;

namespace Studio.Core.StateMachine
{
    public sealed class StateMachine
    {
        private readonly Dictionary<string, IState> _states = new();
        private readonly Stack<IState> _stateStack = new();

        public IState CurrentState { get; private set; }

        public void RegisterState(IState state)
        {
            _states[state.StateId] = state;
        }

        public bool TryGetState(string stateId, out IState state)
        {
            return _states.TryGetValue(stateId, out state);
        }

        public void ChangeState(string stateId)
        {
            if (!_states.TryGetValue(stateId, out var nextState))
            {
                UnityEngine.Debug.LogError($"State '{stateId}' is not registered.");
                return;
            }

            CurrentState?.OnExit();
            _stateStack.Clear();
            CurrentState = nextState;
            CurrentState.OnEnter();
        }

        public void PushState(string stateId)
        {
            if (!_states.TryGetValue(stateId, out var nextState))
            {
                UnityEngine.Debug.LogError($"State '{stateId}' is not registered.");
                return;
            }

            CurrentState?.OnExit();
            if (CurrentState != null)
            {
                _stateStack.Push(CurrentState);
            }

            CurrentState = nextState;
            CurrentState.OnEnter();
        }

        public void PopState()
        {
            if (_stateStack.Count == 0)
            {
                return;
            }

            CurrentState?.OnExit();
            CurrentState = _stateStack.Pop();
            CurrentState.OnEnter();
        }

        public void Tick(float deltaTime)
        {
            CurrentState?.OnTick(deltaTime);
        }
    }
}
