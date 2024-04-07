using System.Net.Sockets;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class EqualityTests
{
    public static object[][] TestData_EqualityComparer_Equal =>
        new object[][]
        {
            [Option.Some(42), Option.Some(42)],
            [Option.Some("test"), Option.Some("test")],
            [Option.Some(true), Option.Some(true)],
            [Option.Some(false), Option.Some(false)],
            [Option.Some(42f), Option.Some(42f)],
            [Option.Some<object?>(null), null],
            [Option.Some<bool?>(null), null],
            [null, null],
            [Option.None<bool>(), Option.None<bool>()],
            [Option.None<bool>(), Option.None<float>()]
        };

    public static object[][] TestData_EqualityComparer_NotEqual =>
        new object[][]
        {
            [Option.Some(42), Option.Some(69)],
            [Option.Some(42), Option.None<int>()],
            [Option.Some(true), Option.Some(false)],
            [Option.Some(new object()), Option.Some<object?>(null)],
            [Option.Some(new object()), null],
            [Option.Some("test"), Option.Some("test2")],
            [Option.Some(42f), Option.Some(43f)]
        };

    [DataTestMethod]
    [DynamicData(nameof(TestData_EqualityComparer_Equal))]
    public void Test_EqualityComparer_Equal(Option lhs, Option rhs)
    {
        OptionEqualityComparer.Default.Equals(lhs, rhs).Should().BeTrue();
    }

    [TestMethod]
    public void Test_EqualityComparer_Generic_Equal()
    {
        OptionEqualityComparer<int>
            .Default.Equals(Option.Some(42), Option.Some(42))
            .Should()
            .BeTrue();
        OptionEqualityComparer<string>
            .Default.Equals(Option.Some("test"), Option.Some("test"))
            .Should()
            .BeTrue();
        OptionEqualityComparer<float>
            .Default.Equals(Option.Some(1f), Option.Some(1f))
            .Should()
            .BeTrue();
        var vObject = new object();
        OptionEqualityComparer<object>.Default.Equals(Option.Some(vObject), Option.Some(vObject));
        object? vNull = null;
        OptionEqualityComparer<object?>
            .Default.Equals(Option.Some(vNull), Option.Some(vNull))
            .Should()
            .BeTrue();
        OptionEqualityComparer<object?>.Default.Equals(Option.Some(vNull), null).Should().BeTrue();
        OptionEqualityComparer<object?>.Default.Equals(null, Option.Some(vNull)).Should().BeTrue();
        OptionEqualityComparer<object?>.Default.Equals(null, null).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_EqualityComparer_NotEqual))]
    public void Test_EqualityComparer_NotEqual(Option lhs, Option rhs)
    {
        OptionEqualityComparer.Default.Equals(lhs, rhs).Should().BeFalse();
    }
}
