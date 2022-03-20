﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Regulus.Extensions
{
    public static class EmptyDelegate
    {
        public static void NewEmpty<T>(this System.Action<T> this_action)
        {
            this_action += _Empty;
        }

        private static void _Empty<T>(T obj)
        {
        }
    }
}
namespace Regulus.Utility
{




    public class AsyncExecuter
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<Action> _Tasks;
        private readonly System.Threading.Tasks.Task _Task;
        private volatile bool _Enable;
        private readonly ManualResetEvent _ResetEvent;
        public AsyncExecuter()
        {
            _ResetEvent = new ManualResetEvent(true);
            _Enable = true;
            _Tasks = new System.Collections.Concurrent.ConcurrentQueue<Action>();
            _Task = new System.Threading.Tasks.Task(_Run , TaskCreationOptions.LongRunning);
            _Task.Start();
        }

        private void _Run()
        {

            while (_Enable)
            {

                Action action;
                if (_Tasks.TryDequeue(out action))
                {
                    action();
                }
                else
                {
                    _ResetEvent.Reset();
                    _ResetEvent.WaitOne();
                }
            }


            _ExecuteAll();
        }

        private void _ExecuteAll()
        {
            Action action;
            while (_Tasks.TryDequeue(out action))
            {
                action();
            }
        }

        public void Shutdown()
        {
            _Enable = false;
            _ResetEvent.Set();
            _Task.Wait();
        }



        public void Push(Action callback)
        {
            _Tasks.Enqueue(callback);
            _ResetEvent.Set();
        }





    }
}
