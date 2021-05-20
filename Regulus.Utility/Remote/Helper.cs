using System;
using System.Threading;

namespace Regulus.Remote
{
    public static class Helper
    {
        public static Action<Value<T>> UnBox<T>(Action<T> callback)
        {
            return val => { val.OnValue += callback; };
        }

        public static T Result<T>(this Value<T> value)
        {
            WaitHandle handle = new AutoResetEvent(false);
            ValueWaiter<T> valueSpin = new ValueWaiter<T>(value);
            ThreadPool.QueueUserWorkItem(valueSpin.Run, handle);
            WaitHandle.WaitAll(
                new[]
                {
                    handle
                });
            return valueSpin.Value;
        }
    }
}
