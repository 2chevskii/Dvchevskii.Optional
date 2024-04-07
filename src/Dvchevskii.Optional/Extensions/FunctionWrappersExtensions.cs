using System;

namespace Dvchevskii.Optional.Extensions
{
    public static class FunctionWrappersExtensions
    {

        public static Option<Unit.Unit> CallSafe(this Action action)
        {
            try
            {
                action();
                return Option.Some(Unit.Unit.Default);
            }
            catch (Exception _)
            {
                return Option.None<Unit.Unit>();
            }
        }

        public static Func<Option<Unit.Unit>> MakeSafe(this Action action)
        {
            return () =>
            {
                try
                {
                    action();
                    return Option.Some(Unit.Unit.Default);
                }
                catch (Exception _)
                {
                    return Option.None<Unit.Unit>();
                }
            };
        }

        public static Func<Option<TResult>> MakeSafeNoExcept<TResult>(this Func<TResult> func)
        {
            return () =>
            {
                try
                {

                    return Option.Some(func());
                }
                catch (Exception _)
                {
                    return Option.None<TResult>();
                }
            };
        }

        public static Func<Option<TResult>, Exception> MakeSafe<TResult>(this Func<TResult> func)
        {
            throw new NotImplementedException();
        }

        public static Option<TResult> Apply<T1, TResult>(this Func<T1, TResult> func, Option<T1> arg0)
        {
            if (arg0.IsNone)
            {
                return Option.None<TResult>();
            }

            return Option.Some(func(arg0.Unwrap()));
        }
    }
}
