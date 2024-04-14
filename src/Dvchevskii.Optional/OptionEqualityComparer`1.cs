using System;
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
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return rhs.IsSome && ReferenceEquals(rhs.Unwrap(), null);
            }

            if (ReferenceEquals(rhs, null))
            {
                return lhs.IsSome && ReferenceEquals(lhs.Unwrap(), null);
            }

            if (lhs.IsNone)
            {
                return rhs.IsNone;
            }

            if (rhs.IsNone)
            {
                return lhs.IsNone;
            }

            T lhsValue = lhs.Unwrap();
            T rhsValue = rhs.Unwrap();

            return EqualityComparer<T>.Default.Equals(lhsValue, rhsValue);
        }

        public bool Equals(Option<T> lhs, Option rhs)
        {
            if (rhs is Option<T> rhsT)
            {
                return Equals(lhs, rhsT);
            }

            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return rhs.IsSome && ReferenceEquals(rhs.UnwrapAny(), null);
            }

            if (ReferenceEquals(rhs, null))
            {
                return lhs.IsSome && ReferenceEquals(lhs.Unwrap(), null);
            }

            if (lhs.IsNone)
            {
                return rhs.IsNone;
            }

            if (rhs.IsNone)
            {
                return lhs.IsNone;
            }

            T lhsValue = lhs.Unwrap();
            object rhsValue = rhs.UnwrapAny();

            if (rhsValue is T rhsValueT)
            {
                return Equals(lhs, rhsValueT);
            }

            return EqualityComparer<object>.Default.Equals(lhsValue, rhsValue);
        }

        public bool Equals(Option<T> lhs, T rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            if (ReferenceEquals(rhs, null))
            {
                return lhs.IsSome && ReferenceEquals(lhs.Unwrap(), null);
            }

            if (lhs.IsNone)
            {
                return false;
            }

            var lhsValue = lhs.Unwrap();

            return EqualityComparer<T>.Default.Equals(lhsValue, rhs);
        }

        public bool Equals(Option<T> lhs, object rhs)
        {
            switch (rhs)
            {
                case Option<T> rhsOptionT:
                    return Equals(lhs, rhsOptionT);
                case Option rhsOption:
                    return Equals(lhs, rhsOption);
                case T rhsT:
                    return Equals(lhs, rhsT);
            }

            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            T lhsValue = lhs.Unwrap();

            return EqualityComparer<object>.Default.Equals(lhsValue, rhs);
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
