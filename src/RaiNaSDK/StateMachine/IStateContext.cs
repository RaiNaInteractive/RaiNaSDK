namespace RaiNa.StateMachine
{
    public interface IStateContext { }

    public delegate void StateMachineDelegate<TContext>
    (
        TContext context,
        IState<TContext> oldState,
        IState<TContext> newState
    ) where TContext : IStateContext;
}
