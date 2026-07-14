using System;

namespace PinionCore.Remote
{
    // 有別於 Notifier，Spirit 是由方法回傳、只供給一次的通知器。
    // Soul 端以 Spirit(T instance) 包裝實例回傳；Ghost 端由生成程式碼以 Spirit() 建立，經 IValue 收到遠端代理（Ghost）。
    // Dispose 後觸發 Unsupply，且不再觸發 Supply。
    public class Spirit<T> : IDisposable, IValue, ISpiritSoul, ISpiritGhost
    {
        readonly object _Lock;
        T _Value;
        bool _HasValue;
        bool _Disposed;
        bool _Unsupplied;
        bool _Dead;

        event Action<T> _Supply;
        event Action<T> _Unsupply;
        event Action _DisposeEvent;

        // Ghost 端（生成程式碼）使用
        public Spirit()
        {
            _Lock = new object();
        }

        // Soul 端（使用者程式碼）使用
        public Spirit(T instance)
        {
            _Lock = new object();
            _Value = instance;
            _HasValue = true;
        }

        public event Action<T> Supply
        {
            add
            {
                bool invoke;
                lock (_Lock)
                {
                    _Supply += value;
                    // 補發語意：已供給且尚未撤銷時，晚訂閱也能收到
                    invoke = _HasValue && !_Disposed && !_Unsupplied && !_Dead;
                }
                if (invoke)
                    value(_Value);
            }
            remove
            {
                lock (_Lock)
                    _Supply -= value;
            }
        }

        public event Action<T> Unsupply
        {
            add
            {
                bool invoke;
                lock (_Lock)
                {
                    _Unsupply += value;
                    // 補發語意：已撤銷時，晚訂閱也能收到
                    invoke = _HasValue && _Unsupplied;
                }
                if (invoke)
                    value(_Value);
            }
            remove
            {
                lock (_Lock)
                    _Unsupply -= value;
            }
        }

        public void Dispose()
        {
            Action disposeEvent;
            lock (_Lock)
            {
                if (_Disposed)
                    return;
                _Disposed = true;
                disposeEvent = _DisposeEvent;
            }
            disposeEvent?.Invoke();
            _RaiseUnsupply();
        }

        void _RaiseUnsupply()
        {
            Action<T> handlers;
            T val;
            lock (_Lock)
            {
                if (!_HasValue || _Unsupplied)
                    return;
                _Unsupplied = true;
                handlers = _Unsupply;
                val = _Value;
            }
            handlers?.Invoke(val);
        }

        bool _SetValue(T val)
        {
            Action<T> handlers;
            lock (_Lock)
            {
                // 單次供給：已有值、已 Dispose 或已錯誤者不再供給
                if (_HasValue || _Disposed || _Dead)
                    return false;
                _Value = val;
                _HasValue = true;
                handlers = _Supply;
            }
            handlers?.Invoke(val);
            return true;
        }

        object IValue.GetObject()
        {
            return _Value;
        }

        Type IValue.GetObjectType()
        {
            return typeof(T);
        }

        bool IValue.IsInterface()
        {
            return true;
        }

        void IValue.QueryValue(Action<object> action)
        {
            if (_HasValue)
                action(_Value);
        }

        bool IValue.SetValue(IGhost ghost)
        {
            return _SetValue((T)ghost);
        }

        bool IValue.SetValue(object val)
        {
            return _SetValue((T)val);
        }

        bool IValue.SetError(string message)
        {
            lock (_Lock)
            {
                if (_HasValue || _Dead)
                    return false;
                _Dead = true;
            }
            return true;
        }

        bool ISpiritSoul.Disposed
        {
            get
            {
                lock (_Lock)
                    return _Disposed;
            }
        }

        event Action ISpiritSoul.DisposeEvent
        {
            add
            {
                bool invoke;
                lock (_Lock)
                {
                    _DisposeEvent += value;
                    invoke = _Disposed;
                }
                if (invoke)
                    value();
            }
            remove
            {
                lock (_Lock)
                    _DisposeEvent -= value;
            }
        }

        void ISpiritGhost.Unsupply()
        {
            _RaiseUnsupply();
        }
    }
}
