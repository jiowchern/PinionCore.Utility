using System;
using System.Collections.Generic;
using System.Linq;

namespace PinionCore.Remote
{
    public class TProvider<T> : INotifier<T>, IProvider
    {

        // 以 List 儲存 handler 而非多播委派欄位：INotifier<out T> 允許以父介面訂閱，
        // 此時傳入的委派執行期型別是 Action<父介面>，Delegate.Combine 會因型別不同而擲出例外。
        private readonly List<Action<T>> _SupplyHandlers;

        private readonly List<Action<T>> _UnsupplyHandlers;

        private readonly List<T> _Entitys;



        private readonly List<T> _Waits;


        public TProvider()
        {
            _Waits = new List<T>();
            _Entitys = new List<T>();
            _SupplyHandlers = new List<Action<T>>();
            _UnsupplyHandlers = new List<Action<T>>();
        }
        event Action<T> INotifier<T>.Supply
        {
            add
            {

                _SupplyHandlers.Add(value);

                lock (_Entitys)
                {
                    foreach (T e in _Entitys.ToArray())
                    {
                        value(e);
                    }
                }
            }

            remove { _SupplyHandlers.Remove(value); }
        }

        event Action<T> INotifier<T>.Unsupply
        {
            add { _UnsupplyHandlers.Add(value); }
            remove { _UnsupplyHandlers.Remove(value); }
        }

        void _NotifySupply(T entity)
        {
            foreach (Action<T> handler in _SupplyHandlers.ToArray())
            {
                handler(entity);
            }
        }

        void _NotifyUnsupply(T entity)
        {
            foreach (Action<T> handler in _UnsupplyHandlers.ToArray())
            {
                handler(entity);
            }
        }

        IGhost IProvider.Ready(long id)
        {
            T entity = (from e in _Waits where (e as IGhost).GetID() == id select e).FirstOrDefault();
            _Waits.Remove(entity);
            if (entity != null)
            {
                return _Add(entity, entity as IGhost);
            }
            PinionCore.Utility.Log.Instance.WriteInfo($"No loaded ghost was found. {typeof(T)}.{id}");
            return null;
        }

        void IProvider.Add(IGhost entity)
        {
            _Waits.Add((T)entity);
        }

        void IProvider.Remove(long id)
        {
            _RemoveEntitys(id);

            _RemoveWaits(id);
        }

        void IProvider.ClearGhosts()
        {


            foreach (T e in _Entitys)
            {
                _NotifyUnsupply(e);
            }

            _Entitys.Clear();
            _Waits.Clear();

        }

        private IGhost _Add(T entity, IGhost ghost)
        {
            if (ghost.IsReturnType() == false)
            {
                lock (_Entitys)
                    _Entitys.Add(entity);
                _NotifySupply(entity);
            }


            return ghost;
        }



        private void _RemoveWaits(long id)
        {
            T waitentity = (from e in _Waits where (e as IGhost).GetID() == id select e).FirstOrDefault();
            if (waitentity != null)
            {
                _Waits.Remove(waitentity);
            }
        }

        private void _RemoveEntitys(long id)
        {
            lock (_Entitys)
            {
                T entity = (from e in _Entitys where (e as IGhost).GetID() == id select e).FirstOrDefault();
                if (entity != null)
                {
                    _Entitys.Remove(entity);

                    _NotifyUnsupply(entity);
                }




            }
        }


    }
}
