using System.Numerics;

namespace Option.Extensions
{
    public class OptionParser
    {
        public static Option<byte> ParseUInt8(string input)
        {
            if (!byte.TryParse(input, out var value))
            {
                return Option.None<byte>();
            }

            return Option.Some(value);
        }

        public static Option<sbyte> ParseInt8(string input)
        {
            if (!sbyte.TryParse(input, out var value))
            {
                return Option.None<sbyte>();
            }

            return Option.Some(value);
        }

        public static Option<ushort> ParseUInt16(string input)
        {
            if (!ushort.TryParse(input, out var value))
            {
                return Option.None<ushort>();
            }

            return Option.Some(value);
        }

        public static Option<short> ParseInt16(string input)
        {
            if (!short.TryParse(input, out var value))
            {
                return Option.None<short>();
            }

            return Option.Some(value);
        }

        public static Option<uint> ParseUInt32(string input)
        {
            if (!uint.TryParse(input, out var value))
            {
                return Option.None<uint>();
            }

            return Option.Some(value);
        }

        public static Option<int> ParseInt32(string input)
        {
            if (!int.TryParse(input, out var value))
            {
                return Option.None<int>();
            }

            return Option.Some(value);
        }

        public static Option<ulong> ParseUInt64(string input)
        {
            if (!ulong.TryParse(input, out var value))
            {
                return Option.None<ulong>();
            }

            return Option.Some(value);
        }

        public static Option<long> ParseInt64(string input)
        {
            if (!long.TryParse(input, out var value))
            {
                return Option.None<long>();
            }

            return Option.Some(value);
        }

        public static Option<BigInteger> ParseBigInt(string input)
        {
            if (!BigInteger.TryParse(input, out var value))
            {
                return Option.None<BigInteger>();
            }

            return Option.Some(value);
        }

        public static Option<bool> ParseBool(string input)
        {
            if (!bool.TryParse(input, out var value))
            {
                return Option.None<bool>();
            }

            return Option.Some(value);
        }

        public static Option<decimal> ParseDecimal(string input)
        {
            if (!decimal.TryParse(input, out var value))
            {
                return Option.None<decimal>();
            }

            return Option.Some(value);
        }


    }
}
