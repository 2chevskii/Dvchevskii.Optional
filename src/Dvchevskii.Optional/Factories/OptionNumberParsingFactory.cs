using System;
using System.Globalization;

namespace Dvchevskii.Optional.Factories
{
    public delegate bool OptionNumberParsingFactory<TOutput>(
        string input,
        NumberStyles numberStyles,
        IFormatProvider formatProvider,
        out TOutput output
    );
}
