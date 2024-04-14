using System.Collections.Generic;

namespace Dvchevskii.Optional
{
    public class OptionEqualityComparer : IEqualityComparer<Option>
    {
        internal const int NONE_HASHCODE = 500029;
        public static readonly OptionEqualityComparer Default = new OptionEqualityComparer();

        public bool Equals(Option lhs, Option rhs)
        {
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
                return lhs.IsSome && ReferenceEquals(lhs.UnwrapAny(), null);
            }

            if (lhs.IsNone)
            {
                return rhs.IsNone;
            }

            if (rhs.IsNone)
            {
                return lhs.IsNone;
            }

            object lhsValue = lhs.UnwrapAny();
            object rhsValue = rhs.UnwrapAny();

            return EqualityComparer<object>.Default.Equals(lhsValue, rhsValue);
        }

        public bool Equals(Option lhs, object rhs)
        {
            if (rhs is Option rhsOption)
            {
                return Equals(lhs, rhsOption);
            }

            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            if (lhs.IsNone)
            {
                return false;
            }

            object lhsValue = lhs.UnwrapAny();

            return EqualityComparer<object>.Default.Equals(lhsValue, rhs);
        }

        public int GetHashCode(Option obj)
        {
            if (obj.IsNone)
            {
                return NONE_HASHCODE;
            }

            int hash = 17;
            hash = hash * 23 + obj.UnderlyingType.GetHashCode();
            hash = hash * 23 + EqualityComparer<object>.Default.GetHashCode(obj.UnwrapAny());
            return hash;
        }
    }
}
