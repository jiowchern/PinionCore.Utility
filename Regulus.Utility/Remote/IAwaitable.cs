using System.Runtime.CompilerServices;

namespace Regulus.Remote
{
    public interface IAwaitable<T> : INotifyCompletion
    {
        
        bool IsCompleted { get; }

        T GetResult();        
    }
}