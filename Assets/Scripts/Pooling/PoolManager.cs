using System.Collections.Generic;

namespace Pooling
{
    public static class PoolManager
    {
        public static Pool<T> CreatePool<T>(Pool<T>.InstantiateDelegate instantiate, Pool<T>.BeforeActivateDelegate beforeActivate, Pool<T>.BeforeDeactivateDelegate beforeDeactivate, int prefill = 10) where T : class, IPoolable
        {
            return new Pool<T>(instantiate, beforeActivate, beforeDeactivate, prefill);
        }
    }

    public class Pool<T> where T : class, IPoolable
    {
        public delegate T InstantiateDelegate();
        public delegate void BeforeActivateDelegate(T item);
        public delegate void BeforeDeactivateDelegate(T item);

        private readonly List<T> _active = new List<T>();
        private readonly List<T> _inactive = new List<T>();

        private readonly InstantiateDelegate _instantiate;
        private readonly BeforeActivateDelegate _beforeActivate;
        private readonly BeforeDeactivateDelegate _beforeDeactivate;
    
        public Pool(InstantiateDelegate instantiate, BeforeActivateDelegate beforeActivate, BeforeDeactivateDelegate beforeDeactivate, int prefill)
        {
            _instantiate = instantiate;
            _beforeActivate = beforeActivate;
            _beforeDeactivate = beforeDeactivate;

            for (var i = 0; i < prefill; i++)
            {
                var obj = _instantiate();
                lock (_inactive) _inactive.Add(obj);
            }
        }

        public T Get()
        {
            T item = null;
            lock (_inactive)
            {
                if (_inactive.Count > 0)
                {
                    item = _inactive[0];
                    _inactive.RemoveAt(0);
                }
            }

            if (item == null)
                item = _instantiate();

            _beforeActivate?.Invoke(item);
            item.OnActivate();
            lock (_active)
            {
                _active.Add(item);
            }
            return item;
        }

        public void Release(T item)
        {
            _beforeDeactivate?.Invoke(item);
            item.OnReset();
            item.OnDeactivate();

            lock (_active) _active.Remove(item);
            lock (_inactive) _inactive.Add(item);
        }
    }
}