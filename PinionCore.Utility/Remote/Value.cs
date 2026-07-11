using System;
using System.Diagnostics;
using System.Dynamic;

namespace PinionCore.Remote
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Value<T> : IValue, IAwaitableSource<T>
    {
        // Go 風格結果通道:error 為 null 代表成功
        private event Action<T, string> _OnValue;



        public event Action<T, string> OnValue
        {
            add
            {
                lock (this)
                {
                    _OnValue += value;

                    if (_Empty == false)
                    {
                        value(_Value, _Error);
                    }
                }

            }

            remove { _OnValue -= value; }
        }



        private readonly bool _Interface;

        private bool _Empty;

        private T _Value;

        private string _Error;


        public static Value<T> Empty
        {
            get { return default(T); }
        }



        public Value(bool empty = true)
        {

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
                OnValue += (obj, error) => { action.Invoke(obj); };
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

        public string GetError()
        {
            return _Error;
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
                _OnValue(_Value, null);
            }

            return true;
        }

        public bool SetError(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            lock (this)
            {
                if (_Empty == false)
                {
                    return false;
                }
                _Empty = false;
                _Error = message;
            }

            if (_OnValue != null)
            {
                _OnValue(_Value, _Error);
            }

            return true;
        }



        bool IValue.SetValue(IGhost ghost)
        {
            return SetValue((T)ghost);
        }

        public IAwaitable<T> GetAwaiter()
        {
            return new ValueAwaiter<T>(this);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
    public class Value : IValue, IAwaitableSource
    {
        bool _Empty;
        string _Error;
        public Value(bool empty )
        {
            _OnValue = (error) => { };
            _Empty = empty;
        }


        object IValue.GetObject()
        {
            return null;
        }

        Type IValue.GetObjectType()
        {
            return typeof(void);
        }

        bool IValue.IsInterface()
        {
            return false;        }

        void IValue.QueryValue(Action<object> action)
        {
            if (_Empty == false)
            {
                action.Invoke(null);
            }
            else
            {
                OnValue += (error) => { action.Invoke(null); };
            }
        }

        bool IValue.SetValue(object val)
        {
            return _SetValue();
        }

        private bool _SetValue()
        {
            if (_Empty == false)
            {
                return false;
            }
            _Empty = false;
            _OnValue(null);
            return true;
        }

        public bool SetError(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (_Empty == false)
            {
                return false;
            }
            _Empty = false;
            _Error = message;
            _OnValue(_Error);
            return true;
        }

        public string GetError()
        {
            return _Error;
        }

        System.Action<string> _OnValue;
        public event System.Action<string> OnValue
        {
            add
            {
                lock (this)
                {
                    _OnValue += value;
                    if (_Empty == false)
                    {
                        value(_Error);
                    }
                }
            }
            remove { _OnValue -= value; }
        }

        bool IValue.SetValue(IGhost ghost)
        {
            return _SetValue();
        }

        IAwaitable IAwaitableSource.GetAwaitable()
        {
            return new ValueAwaiter(this);
        }

        internal bool HasValue()
        {
            return _Empty == false;

        }
        public IAwaitable GetAwaiter()
        {
            return new ValueAwaiter(this);
        }
    }
}
