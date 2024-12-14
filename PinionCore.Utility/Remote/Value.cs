using System;
using System.Runtime.CompilerServices;

namespace PinionCore.Remote
{

    public class ValueAWaiter<T> : IAwaitable<T>
    {
        
        readonly Value<T> _Value;
        readonly System.Collections.Concurrent.ConcurrentQueue<T> _Invokeds;

        public ValueAWaiter(Value<T> value)
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

    public class Value<T> : IValue,  IWaitableValue<T>
    {
        private event Action<T> _OnValue;

        readonly System.Collections.Concurrent.ConcurrentQueue<T> _Invokeds;

        public event Action<T> OnValue
        {
            add
            {
                lock (this)
                {
                    _OnValue += value;

                    if (_Empty == false)
                    {
                        value(_Value);
                    }
                }

            }

            remove { _OnValue -= value; }
        }



        private readonly bool _Interface;

        private bool _Empty;

        private T _Value;


        public static Value<T> Empty
        {
            get { return default(T); }
        }

         

        public Value(bool empty = true)
        {
            _Invokeds = new System.Collections.Concurrent.ConcurrentQueue<T>();
            _Empty = empty;
            _Interface = typeof(T).IsInterface;
        }


        public Value(T val) : this(false)
        {

            _Value = val;
        }

        object IValue.GetObject()
        {
            return _Value;
        }



        bool IValue.SetValue(object val)
        {
            return SetValue((T)val);
        }

        void IValue.QueryValue(Action<object> action)
        {
            if (_Empty == false)
            {
                action.Invoke(_Value);
            }
            else
            {
                OnValue += obj => { action.Invoke(obj); };
            }
        }

        bool IValue.IsInterface()
        {
            return _Interface;
        }

        Type IValue.GetObjectType()
        {
            return typeof(T);
        }



        public static implicit operator Value<T>(T value)
        {
            return new Value<T>(value);
        }

        public T GetValue()
        {
            return _Value;

        }

        public bool HasValue()
        {
            return _Empty == false;
        }


        public bool SetValue(T val)
        {
            lock (this)
            {
                if (_Empty == false)
                {
                    return false;
                }
                _Empty = false;
                _Value = val;
            }


            
            if (_OnValue != null)
            {
                _OnValue(_Value);
            }

            return true;
        }

         

        bool IValue.SetValue(IGhost ghost)
        {
            return SetValue((T)ghost);
        }
      
        public IAwaitable<T> GetAwaiter()
        {
            return new ValueAWaiter<T>(this);
        }
    }
}
