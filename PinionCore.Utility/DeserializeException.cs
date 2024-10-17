using System;

namespace PinionCore
{
    public class DeserializeException : Exception
    {
        private readonly Exception _Exception;

        public DeserializeException(Exception exception)
        {
            _Exception = exception;
        }

        public Exception Base { get { return _Exception; } }
    }
}