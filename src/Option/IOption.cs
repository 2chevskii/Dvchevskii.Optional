using System;

namespace Option
{
    public interface IOption : IEquatable<IOption>
    {
        bool IsNone();
        bool IsSome();

        Type GetUnderlyingType();
    }
}
