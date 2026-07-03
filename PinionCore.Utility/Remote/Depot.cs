using System;
using System.Collections;
using System.Collections.Generic;

namespace PinionCore.Remote
{
    public class Depot<T> : INotifier<T>, ICollection<T>, System.Collections.Generic.IReadOnlyCollection<T>
    {
        readonly System.Collections.Generic.List<T> _Items;

        // 以 List 儲存 handler 而非多播委派欄位：INotifier<out T> 允許以父介面訂閱，
        // 此時傳入的委派執行期型別是 Action<父介面>，Delegate.Combine 會因型別不同而擲出例外。
        readonly System.Collections.Generic.List<Action<T>> _SupplyHandlers;
        readonly System.Collections.Generic.List<Action<T>> _UnsupplyHandlers;


        int ICollection<T>.Count => _Items.Count;

        bool ICollection<T>.IsReadOnly => Items.IsReadOnly;

        int IReadOnlyCollection<T>.Count => _Items.Count;

        public readonly ICollection<T> Items;
        public readonly IReadOnlyCollection<T> ReadOnlyItems;
        public readonly INotifier<T> Notifier;
        
        public Depot() : this(new System.Collections.Generic.List<T>())
        {
        }
        public Depot(IEnumerable<T> items)
        {
            _Items = new System.Collections.Generic.List<T>(items);
            _SupplyHandlers = new System.Collections.Generic.List<Action<T>>();
            _UnsupplyHandlers = new System.Collections.Generic.List<Action<T>>();
            Items = this;
            ReadOnlyItems = this;
            Notifier = this;
        }

        event Action<T> INotifier<T>.Supply
        {
            add
            {
                _SupplyHandlers.Add(value);
                foreach (T item in _Items)
                {
                    value(item);
                }
            }

            remove
            {
                _SupplyHandlers.Remove(value);
            }
        }

        event Action<T> INotifier<T>.Unsupply
        {
            add
            {
                _UnsupplyHandlers.Add(value);
            }

            remove
            {
                _UnsupplyHandlers.Remove(value);
            }
        }

        void _NotifySupply(T item)
        {
            foreach (Action<T> handler in _SupplyHandlers.ToArray())
            {
                handler(item);
            }
        }

        void _NotifyUnsupply(T item)
        {
            foreach (Action<T> handler in _UnsupplyHandlers.ToArray())
            {
                handler(item);
            }
        }

        void ICollection<T>.Add(T item)
        {
            _Items.Add(item);
            _NotifySupply(item);
        }

        void ICollection<T>.Clear()
        {
            foreach (T item in _Items)
            {
                _NotifyUnsupply(item);
            }
            _Items.Clear();

        }

        bool ICollection<T>.Contains(T item)
        {
            return _Items.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            _Items.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {

            var result = _Items.Remove(item);
            if (result)
                _NotifyUnsupply(item);
            return result;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }
    }
}
