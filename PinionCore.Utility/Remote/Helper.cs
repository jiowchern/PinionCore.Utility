using System;
using System.Threading;

namespace PinionCore.Remote
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
            var valueSpin = new ValueWaiter<T>(value);
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
