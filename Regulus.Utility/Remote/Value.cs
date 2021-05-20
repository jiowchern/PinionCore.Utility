using System;
using System.Runtime.CompilerServices;

namespace Regulus.Remote
{


    /// <summary>
    ///     接收或傳送遠端來的資料
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Value<T> : IValue ,  IAwaitable<T>
    {
        private event Action<T> _OnValue;
        Action _Continuation;

        /// <summary>
        ///     如果有設定資料則會發生此事件
        /// </summary>
        public event Action<T> OnValue
        {
            add
            {
                _OnValue += value;

                if (_Empty == false)
                {
                    value(_Value);
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

        bool IAwaitable<T>.IsCompleted => HasValue();

        public Value()
        {
            _Continuation = () => { };
            _Empty = true;
            _Interface = typeof(T).IsInterface;
        }

        
        public Value(T val) : this()
        {
            _Empty = false;
            _Value = val;
            if (_OnValue != null)
            {
                _OnValue(_Value);
            }
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
            if (_Empty == false)
            {
                return false;
            }
            _Empty = false;
            _Value = val;
            _Continuation();
            if (_OnValue != null)
            {
                _OnValue(_Value);
            }
            
            return true;
        }

        /// <summary>
        ///     取得資料
        /// </summary>
        /// <param name="val"></param>
        /// <returns>如果有資料則傳回真</returns>
        public bool TryGetValue(out T val)
        {
            if (_Empty == false)
            {
                val = _Value;
                return true;
            }

            val = default(T);
            return false;
        }

        public IAwaitable<T> GetAwaiter()
        {
            return this;
        }

        bool IValue.SetValue(IGhost ghost)
        {
            return SetValue((T)ghost);
        }

        T IAwaitable<T>.GetResult()
        {
            return GetValue();
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            _Continuation = continuation;
        }
    }
}
