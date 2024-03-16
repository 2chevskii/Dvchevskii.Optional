using System;
using System.Globalization;
using System.Numerics;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Dvchevskii.Optional.Extensions
{
    public static class StringExtensions
    {
        public static Option<bool> ToBoolOption(this string self) =>
            OptionParsingUtil.ParseBool(self);

        public static Option<byte> ToUInt8Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseUInt8(self, numberStyles, formatProvider);

        public static Option<sbyte> ToInt8Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseInt8(self, numberStyles, formatProvider);

        public static Option<ushort> ToUInt16Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseUInt16(self, numberStyles, formatProvider);

        public static Option<short> ToInt16Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseInt16(self, numberStyles, formatProvider);

        public static Option<uint> ToUInt32Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseUInt32(self, numberStyles, formatProvider);

        public static Option<int> ToInt32Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseInt32(self, numberStyles, formatProvider);

        public static Option<ulong> ToUInt64Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseUInt64(self, numberStyles, formatProvider);

        public static Option<long> ToInt64Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseInt64(self, numberStyles, formatProvider);

        public static Option<BigInteger> ToBigIntOption(
            this string self,
            NumberStyles numberStyles = NumberStyles.Integer,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseBigInt(self, numberStyles, formatProvider);

        public static Option<decimal> ToDecimalOption(
            this string self,
            NumberStyles numberStyles = NumberStyles.Number,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseDecimal(self, numberStyles, formatProvider);

        public static Option<float> ToFloat32Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseFloat32(self, numberStyles, formatProvider);

        public static Option<double> ToFloat64Option(
            this string self,
            NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider formatProvider = null
        ) => OptionParsingUtil.ParseFloat64(self, numberStyles, formatProvider);
    }
}
