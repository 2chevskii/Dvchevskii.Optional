using System;

namespace Dvchevskii.Optional.Exceptions
{
    public sealed class ExpectNoneException : Exception
    {
        public ExpectNoneException(string message) : base(message)
        {

        }
    }
}
