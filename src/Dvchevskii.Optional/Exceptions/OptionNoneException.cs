using System;

namespace Dvchevskii.Optional.Exceptions
{
    public sealed class OptionNoneException : InvalidOperationException
    {
        public OptionNoneException() { }

        public OptionNoneException(string message)
            : base(message) { }
    }
}
