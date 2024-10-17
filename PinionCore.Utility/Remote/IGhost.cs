using System.Reflection;

namespace PinionCore.Remote
{
    public delegate void CallMethodCallback(MethodInfo info, object[] args, IValue return_value);
    public delegate void EventNotifyCallback(EventInfo info, long handler_id);

    public delegate void PassageCallback(object gpi);
    public delegate void PropertyNotifierCallback(PropertyInfo info, PassageCallback passage);

    public interface IGhost
    {
        long GetID();

        object GetInstance();

        bool IsReturnType();

        event CallMethodCallback CallMethodEvent;
        event EventNotifyCallback AddEventEvent;
        event EventNotifyCallback RemoveEventEvent;
    }


}
