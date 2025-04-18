using System.Runtime.CompilerServices;

namespace PinionCore.Remote
{
    public interface IAwaitable : INotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }
    public interface IAwaitable<T> : INotifyCompletion
    {

        bool IsCompleted { get; }

        T GetResult();

        
    }
}
