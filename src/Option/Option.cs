namespace Option
{
    public static class Option
    {
        public static Option<T> None<T>() => new None<T>();

        public static Option<T> Some<T>(T value) => new Some<T>(value);
    }
}
