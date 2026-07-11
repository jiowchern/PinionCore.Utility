namespace PinionCore.Remote
{
    // 遠端方法以錯誤完成時(Value<T>.OnValue 的 error 非 null),
    // Reactive 包裝層以此型別發出 OnError,方便呼叫端過濾
    public class RemoteMethodException : System.Exception
    {
        public RemoteMethodException(string message) : base(message)
        {
        }
    }
}
