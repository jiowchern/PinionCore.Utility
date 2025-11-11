using System;
namespace PinionCore.Utility
{
    public sealed class DisposeAction : IDisposable
    {
        private readonly Action _Action;
        public DisposeAction(Action action)
        {
            _Action = action;
        }
        void IDisposable.Dispose()
        {
            _Action();
        }
    }
}
