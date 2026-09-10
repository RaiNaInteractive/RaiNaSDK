using System;
using System.Collections.Generic;

namespace RaiNa.StateMachine
{
    public sealed class StateMachineSubsystem : IStateMachineSubsystem, IDisposable
    {
        private readonly List<IStateMachine> _machines = new();

        public bool Enabled { get; private set; } = true;

        public void Disable() => Enabled = false;
        public void Enable() => Enabled = true;

        public void Register(IStateMachine machine)
        {
            if (!_machines.Contains(machine))
                _machines.Add(machine);
        }

        public void Unregister(IStateMachine machine)
        {
            _machines.Remove(machine);
        }

        public void Update(float deltaTime)
        {
            if (!Enabled)
                return;

            for (int i = 0; i < _machines.Count; i++)
            {
                var m = _machines[i];

                if (m == null)
                    continue;

                if (m.Enabled)
                    m.Update(deltaTime);
            }
        }

        bool _disposed = false;

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (IStateMachine machine in _machines)
                {
                    IDisposable disposable = machine as IDisposable;
                    disposable?.Dispose();
                }

                _machines.Clear();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StateMachineSubsystem()
        {
            Dispose(false);
        }
    }
}
