namespace Dvchevskii.Optional
{
    public abstract partial class Option
    {
        public static bool operator ==(Option lhs, Option rhs) =>
            OptionEqualityComparer.Default.Equals(lhs, rhs);

        public static bool operator !=(Option lhs, Option rhs) =>
            !OptionEqualityComparer.Default.Equals(lhs, rhs);
    }
}
