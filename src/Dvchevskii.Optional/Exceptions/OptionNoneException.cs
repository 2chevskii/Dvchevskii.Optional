using System;

namespace Dvchevskii.Optional.Exceptions
{
    public sealed class OptionIsNoneException : Exception
    {
        public OptionIsNoneException()
        {

        }

        public OptionIsNoneException(string message) : base(message)
        {

        }
    }
}
