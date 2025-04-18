namespace PinionCore.Remote
{

    public interface IAwaitableSource
    {
        IAwaitable GetAwaitable();
    }
    public interface IAwaitableSource<T> 
    {
        IAwaitable<T> GetAwaiter();        
    }
}
