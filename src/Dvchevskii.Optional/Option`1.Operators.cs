namespace Dvchevskii.Optional
{
    public partial class Option<T>
    {
        public static bool operator ==(Option<T> lhs, Option<T> rhs) =>
            OptionEqualityComparer<T>.Default.Equals(lhs, rhs);

        public static bool operator !=(Option<T> lhs, Option<T> rhs) =>
            OptionEqualityComparer<T>.Default.Equals(lhs, rhs);
    }
}
