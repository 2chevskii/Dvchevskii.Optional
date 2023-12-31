using System;

namespace Option.Exceptions
{
    public sealed class NoneOptionException : Exception
    {
        public NoneOptionException(string message) : base(message)
        {

        }
    }
}