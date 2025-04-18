using System;
using System.Runtime.CompilerServices;

namespace PinionCore.Remote
{
    public class ValueAwaiter<T> : IAwaitable<T>
    {

        readonly Value<T> _Value;
        readonly System.Collections.Concurrent.ConcurrentQueue<T> _Invokeds;

        public ValueAwaiter(Value<T> value)
        {
            _Invokeds = new System.Collections.Concurrent.ConcurrentQueue<T>();
            _Value = value;
        }
        bool IAwaitable<T>.IsCompleted => _Value.HasValue();

        T IAwaitable<T>.GetResult()
        {
            return _Value.GetValue();
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            void Handler(T v)
            {
                _Invokeds.Enqueue(v);


                if (_Invokeds.Count > 1)
                {
                    PinionCore.Utility.Log.Instance.WriteInfo($" INotifyCompletion.OnCompleted:{_Invokeds.Count} ");

                    throw new InvalidOperationException("INotifyCompletion.OnCompleted");
                }

                _Value.OnValue -= Handler;

                continuation?.Invoke();

            }
            _Value.OnValue += Handler;
        }
    }
}
