namespace PinionCore.Remote
{
    
    public interface IWaitableValue<T> 
    {
        IAwaitable<T> GetAwaiter();
        event System.Action<T> ValueEvent;
    }
}