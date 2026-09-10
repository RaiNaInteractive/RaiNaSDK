using System;

namespace RaiNa.StateMachine
{
    public sealed class StateMachine<TContext> : IStateMachine, IDisposable where TContext : IStateContext
    {
        public bool Enabled { get; private set; } = true;
        public bool IsTransitioning { get; private set; }

        public event StateMachineDelegate<TContext> OnStateChanged;

        private IState<TContext> _currentState;
        private IState<TContext> _nextState;

        private readonly TContext _context;
        private readonly IStateMachineSubsystem _subsystem;

        private bool _disposed = false;

        public StateMachine(TContext context, IStateMachineSubsystem subsystem)
        {
            _context = context;
            _subsystem = subsystem;
            _subsystem.Register(this);
        }

        public void Disable() => Enabled = false;
        public void Enable() => Enabled = true;

        public void ChangeState<TState>(TState state)
        {
            if (state is not IState<TContext> newState)
                throw new ArgumentException(
                    $"State type '{typeof(TState).Name}' does not implement {typeof(IState<TContext>).Name}.");

            _nextState = newState;
            IsTransitioning = true;
        }

        public void Update(float deltaTime)
        {
            if (!Enabled)
                return;

            ProcessTransition();

            if (IsTransitioning)
                return;

            _currentState?.OnUpdate(_context, deltaTime);
        }

        public void ProcessTransition()
        {
            if (_nextState == null)
                return;

            _currentState?.OnExit(_context);

            var oldState = _currentState ?? _nextState;
            _currentState = _nextState;

            _currentState?.OnEnter(_context);

            OnStateChanged?.Invoke(_context, oldState, _currentState);

            _nextState = null;
            IsTransitioning = false;
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Enabled = false;
                _subsystem.Unregister(this);
                _currentState = null;
                _nextState = null;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StateMachine()
        {
            Dispose(false);
        }
    }
}
