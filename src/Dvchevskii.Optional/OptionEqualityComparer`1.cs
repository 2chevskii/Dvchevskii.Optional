using System.Collections.Generic;

namespace Dvchevskii.Optional
{
    public class OptionEqualityComparer<T> : IEqualityComparer<Option<T>>
    {
        public static readonly OptionEqualityComparer<T> Default = new OptionEqualityComparer<T>();

        public bool Equals(Option<T> lhs, Option<T> rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null) || rhs.IsSome && ReferenceEquals(rhs.Unwrap(), null);
            }

            if (ReferenceEquals(rhs, null))
            {
                return lhs.IsSome && ReferenceEquals(lhs.Unwrap(), null);
            }

            if (lhs.IsSome != rhs.IsSome)
            {
                return false;
            }

            if (lhs.IsNone)
            {
                return true;
            }

            T lhsValue = lhs.Unwrap();
            T rhsValue = rhs.Unwrap();

            return EqualityComparer<T>.Default.Equals(lhsValue, rhsValue);
        }

        public int GetHashCode(Option<T> obj)
        {
            if (obj.IsNone)
            {
                return OptionEqualityComparer.NONE_HASHCODE;
            }

            int hash = 17;
            hash = hash * 23 + obj.UnderlyingType.GetHashCode();
            hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(obj.Unwrap());
            return hash;
        }
    }
}
