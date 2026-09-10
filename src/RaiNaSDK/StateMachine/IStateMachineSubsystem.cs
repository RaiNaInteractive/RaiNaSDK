namespace RaiNa.StateMachine
{
    public interface IStateMachineSubsystem
    {
        bool Enabled { get; }
        void Enable();
        void Disable();
        void Register(IStateMachine machine);
        void Unregister(IStateMachine machine);
        void Update(float deltaTime);
    }
}
