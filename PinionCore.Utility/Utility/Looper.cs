namespace PinionCore.Utility
{
    public class Looper<Item> where Item : class
    {
        public delegate void OnItem(Item item);
        struct Command
        {
            
            public System.Action Invoker;
            
        }
        readonly System.Collections.Concurrent.ConcurrentQueue<Command> _Commands;
        public Looper() 
        {
            _Commands = new System.Collections.Concurrent.ConcurrentQueue<Command>();
            AddItemEvent += _Empty;
            RemoveItemEvent += _Empty;
            UpdateEvent += _Empty;
        }

        private void _Empty()
        {

        }
        private void _Empty(Item item)
        {
            
        }

        public void Add(Item item)
        {
            _Commands.Enqueue(new Command { Invoker =()=> {
                AddItemEvent(item);
            } });
        }
        public void Remove(Item item)
        {
            _Commands.Enqueue(new Command
            {
                Invoker = () => {
                    RemoveItemEvent(item);
                }
            });
        }

        
        public event OnItem AddItemEvent;
        public event OnItem RemoveItemEvent;
        public event System.Action UpdateEvent;
        public void Update()
        {
            Command cmd;
            while (_Commands.TryDequeue(out cmd))
            {
                cmd.Invoker();
            }
            UpdateEvent();
        }
    }
}
