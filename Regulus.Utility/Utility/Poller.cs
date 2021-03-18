using System;
using System.Collections.Generic;

namespace Regulus.Utility
{
    public class Poller<T> where T : class
    {
        
        private readonly List<T> _Objects;
        
        


        public Poller()
        {
            _Objects = new List<T>();
            
        }
        public void Add(T obj)
        {

            lock (_Objects)
                _Objects.Add(obj);
        }

        public void Remove(Func<T, bool> predicate)
        {
            lock (_Objects)
                _Objects.RemoveAll((o) => predicate(o));

        }

        public T[] UpdateSet()
        {
            lock (_Objects)
            {
                return _Objects.ToArray();
            }
        }

      
    }
}
