using System;

namespace Option
{
    public interface IOption : IEquatable<IOption>
    {
        /// <summary>
        /// Check if <see cref="IOption"/> does not contain a value
        /// </summary>
        /// <returns></returns>
        bool IsNone();

        /// <summary>
        /// Check if <see cref="IOption"/> contains a value
        /// </summary>
        /// <returns></returns>
        bool IsSome();

        /// <summary>
        /// Get the underlying type of <see cref="IOption"/>
        /// </summary>
        /// <returns></returns>
        Type GetUnderlyingType();
    }
}
