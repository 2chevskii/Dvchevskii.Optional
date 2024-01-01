using System;

namespace Option.Exceptions
{
    public sealed class ExpectNoneException : Exception
    {
        public ExpectNoneException(string message) : base(message)
        {

        }
    }
}
