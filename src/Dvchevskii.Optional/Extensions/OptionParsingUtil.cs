using System;
using System.Globalization;
using System.Numerics;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Dvchevskii.Optional.Extensions
{
    public static class OptionParsingUtil
    {
        public static Option<bool> ParseBool(string input) =>
            !bool.TryParse(input, out bool value) ? Option.None<bool>() : Option.Some(value);

        public static Option<byte> ParseUInt8(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !byte.TryParse(input, numberStyles, formatProvider, out byte result)
                ? Option.None<byte>()
                : Option.Some(result);

        public static Option<sbyte> ParseInt8(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !sbyte.TryParse(input, numberStyles, formatProvider, out sbyte result)
                ? Option.None<sbyte>()
                : Option.Some(result);

        public static Option<ushort> ParseUInt16(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !ushort.TryParse(input, numberStyles, formatProvider, out ushort result)
                ? Option.None<ushort>()
                : Option.Some(result);

        public static Option<short> ParseInt16(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !short.TryParse(input, numberStyles, formatProvider, out short result)
                ? Option.None<short>()
                : Option.Some(result);

        public static Option<uint> ParseUInt32(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !uint.TryParse(input, numberStyles, formatProvider, out uint result)
                ? Option.None<uint>()
                : Option.Some(result);

        public static Option<int> ParseInt32(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !int.TryParse(input, numberStyles, formatProvider, out int result)
                ? Option.None<int>()
                : Option.Some(result);

        public static Option<ulong> ParseUInt64(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !ulong.TryParse(input, numberStyles, formatProvider, out ulong result)
                ? Option.None<ulong>()
                : Option.Some(result);

        public static Option<long> ParseInt64(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !long.TryParse(input, numberStyles, formatProvider, out long result)
                ? Option.None<long>()
                : Option.Some(result);

        public static Option<BigInteger> ParseBigInt(
            string input,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) =>
            !BigInteger.TryParse(input, numberStyles, formatProvider, out BigInteger result)
                ? Option.None<BigInteger>()
                : Option.Some(result);

        public static Option<decimal> ParseDecimal(
            string input,
            NumberStyles numberStyles = NumberStyles.Number,
            IFormatProvider formatProvider = null
        ) =>
            !decimal.TryParse(input, numberStyles, formatProvider, out decimal result)
                ? Option.None<decimal>()
                : Option.Some(result);

        public static Option<float> ParseFloat32(
            string input,
            NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider formatProvider = null
        ) =>
            !float.TryParse(input, numberStyles, formatProvider, out float result)
                ? Option.None<float>()
                : Option.Some(result);

        public static Option<double> ParseFloat64(
            string input,
            NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider formatProvider = null
        ) =>
            !double.TryParse(input, numberStyles, formatProvider, out double result)
                ? Option.None<double>()
                : Option.Some(result);
    }
}
