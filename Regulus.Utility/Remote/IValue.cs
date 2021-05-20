using System;

namespace Regulus.Remote
{
    public interface IValue
    {
        object GetObject();

        bool SetValue(object val);

        void QueryValue(Action<object> action);

        bool SetValue(IGhost ghost);

        bool IsInterface();

        Type GetObjectType();

    }
}
