using System;
using System.Runtime.CompilerServices;

namespace PinionCore.Remote
{
    public class ValueAwaiter : IAwaitable
    {
        private readonly Value _Value;
        private readonly IValue _Base;
             
        
        public ValueAwaiter(Value val)
        {
            _Base = val;
            _Value = val;
        }
        bool IAwaitable.IsCompleted => _Value.HasValue();

        void IAwaitable.GetResult()
        {
            
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {

            void Handler()
            {
                _Value.OnValue -= Handler;
                continuation?.Invoke();
            }
            _Value.OnValue += Handler;
        }
    }
}
