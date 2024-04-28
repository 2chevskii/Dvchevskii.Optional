using System;
// ReSharper disable MemberCanBePrivate.Global

namespace Dvchevskii.Optional.MethodWrappers
{
    public static partial class MethodWrapperExtensions
    {
        /*public static Option<Unit.Unit> CallSafe(this Action action)
        {
            try
            {
                action.Invoke();
                return Option.Some(Unit.Unit.Default);
            }
            catch
            {
                return Option.None<Unit.Unit>();
            }
        }

        public static Option<TResult> CallSafe<TResult>(this Func<TResult> func)
        {
            try
            {
                return Option.Some(func());
            }
            catch
            {
                return Option.None<TResult>();
            }
        }

        public static Func<Option<Unit.Unit>> MakeSafe(this Action action) => action.CallSafe;*/

        // public static Func<Option<TResult>> MakeSafe<TResult>(this Func<TResult> func)
        // {
        //     try
        //     {
        //         var result = func();
        //         return Option.Some(result);
        //     }
        // }
    }
}
