namespace RaiNa.StateMachine
{
    public interface IState<TContext> where TContext : IStateContext
    {
        void OnEnter(in TContext ctx);
        void OnUpdate(in TContext ctx, float deltaTime);
        void OnExit(in TContext ctx);
    }
}
