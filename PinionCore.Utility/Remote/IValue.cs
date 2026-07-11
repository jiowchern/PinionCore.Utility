using System;

namespace PinionCore.Remote
{
    public interface IValue
    {
        object GetObject();

        bool SetValue(object val);

        void QueryValue(Action<object> action);

        bool SetValue(IGhost ghost);

        // 以錯誤狀態完成:值為 default,錯誤訊息不得為 null
        bool SetError(string message);

        bool IsInterface();

        Type GetObjectType();

    }
}
