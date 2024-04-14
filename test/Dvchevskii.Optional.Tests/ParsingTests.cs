using System.Globalization;
using Dvchevskii.Optional.Extensions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class ParsingTests
{
    [TestMethod]
    public void Test_ParseBool()
    {
        "True".ToBoolOption().Should().Be(Option.Some(true));
        "true".ToBoolOption().Should().Be(Option.Some(true));
        "False".ToBoolOption().Should().Be(Option.Some(false));
        "false".ToBoolOption().Should().Be(Option.Some(false));
        "NotTrue".ToBoolOption().Should().Be(Option.None<bool>());
        "NotFalse".ToBoolOption().Should().Be(Option.None<bool>());
    }

    [TestMethod]
    public void Test_ParseInt32()
    {
        "42".ToInt32Option().Should().Be(Option.Some(42));
        "42.0".ToInt32Option().Should().Be(Option.None<int>());
        "0A".ToInt32Option(NumberStyles.HexNumber).Should().Be(Option.Some(0x0a));
    }
}
