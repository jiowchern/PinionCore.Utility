using System.Runtime.CompilerServices;

namespace PinionCore.Remote
{
    public interface IAwaitable<T> : INotifyCompletion
    {

        bool IsCompleted { get; }

        T GetResult();
    }
}
