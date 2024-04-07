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
                return ReferenceEquals(rhs, null)
                    || rhs.IsSome && ReferenceEquals(rhs.UnwrapAny(), null);
            }

            if (ReferenceEquals(rhs, null))
            {
                return lhs.IsSome && ReferenceEquals(lhs.UnwrapAny(), null);
            }

            if (lhs.IsSome != rhs.IsSome)
            {
                return false;
            }

            if (lhs.IsNone)
            {
                return true;
            }

            object lhsValue = lhs.UnwrapAny();
            object rhsValue = rhs.UnwrapAny();

            return EqualityComparer<object>.Default.Equals(lhsValue, rhsValue);
        }

        public int GetHashCode(Option obj)
        {
            if (obj.IsNone)
            {
                return NONE_HASHCODE;
            }
            unchecked
            {
                int hash = NONE_HASHCODE;

                hash = hash * 23 * obj.UnderlyingType.GetHashCode();

                return hash * (EqualityComparer<object>.Default.GetHashCode(obj.UnwrapAny()) + 3);
            }
        }
    }
}
