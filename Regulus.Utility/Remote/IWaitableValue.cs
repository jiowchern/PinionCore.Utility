namespace Regulus.Remote
{
    
    public interface IWaitableValue<T> 
    {
        IAwaitable<T> GetAwaiter();
        event System.Action<T> ValueEvent;
    }
}