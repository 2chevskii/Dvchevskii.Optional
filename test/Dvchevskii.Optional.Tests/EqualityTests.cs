using FluentAssertions;

// ReSharper disable InconsistentNaming

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
            [Option.Some(42f), Option.Some(43f)],
            [Option.None<object?>(), Option.Some<object?>(null)],
        };

    public static object[][] TestData_Operators_Equal =>
        new object[][]
        {
            [Option.Some(42), Option.Some(42)],
            [Option.Some(true), Option.Some(true)],
            [Option.Some(false), Option.Some(false)],
            [Option.Some(42f), Option.Some(42f)],
            [Option.Some("test"), Option.Some("test")],
            [Option.Some<object?>(null), Option.Some<object?>(null)],
            [Option.None<int>(), Option.None<int>()],
            [Option.None<bool>(), Option.None<object>()],
        };
    public static object[][] TestData_Operators_NotEqual =>
        new object[][]
        {
            [Option.Some(42), Option.Some(69)],
            [Option.Some(true), Option.Some(false)],
            [Option.Some(true), Option.None<bool>()],
            [Option.Some<object?>(null), Option.Some(new object())],
            [Option.Some(42f), Option.Some(69f)]
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
        OptionEqualityComparer<object>
            .Default.Equals(Option.Some(vObject), Option.Some(vObject))
            .Should()
            .BeTrue();
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

    [TestMethod]
    public void Test_EqualityComparer_Generic_NotEqual()
    {
        OptionEqualityComparer<int>
            .Default.Equals(Option.Some(42), Option.Some(24))
            .Should()
            .BeFalse();
        OptionEqualityComparer<int>
            .Default.Equals(Option.Some(42), Option.None<int>())
            .Should()
            .BeFalse();
        OptionEqualityComparer<int>
            .Default.Equals(Option.None<int>(), Option.Some(42))
            .Should()
            .BeFalse();

        OptionEqualityComparer<string>
            .Default.Equals(Option.Some("hello"), Option.Some("world"))
            .Should()
            .BeFalse();
        OptionEqualityComparer<string>
            .Default.Equals(Option.Some("hello"), Option.None<string>())
            .Should()
            .BeFalse();
        OptionEqualityComparer<string>
            .Default.Equals(Option.None<string>(), Option.Some("hello"))
            .Should()
            .BeFalse();

        OptionEqualityComparer<bool>
            .Default.Equals(Option.Some(true), Option.Some(false))
            .Should()
            .BeFalse();
        OptionEqualityComparer<bool>
            .Default.Equals(Option.Some(true), Option.None<bool>())
            .Should()
            .BeFalse();
        OptionEqualityComparer<bool>
            .Default.Equals(Option.None<bool>(), Option.Some(true))
            .Should()
            .BeFalse();

        OptionEqualityComparer<double>
            .Default.Equals(Option.Some(3.14), Option.Some(2.71))
            .Should()
            .BeFalse();
        OptionEqualityComparer<double>
            .Default.Equals(Option.Some(3.14), Option.None<double>())
            .Should()
            .BeFalse();
        OptionEqualityComparer<double>
            .Default.Equals(Option.None<double>(), Option.Some(3.14))
            .Should()
            .BeFalse();
    }

    [TestMethod]
    public void Test_EqualityComparer_GetHashCode()
    {
        var cmp = OptionEqualityComparer.Default;
        cmp.GetHashCode(Option.Some(42)).Should().Be(cmp.GetHashCode(Option.Some(42)));
        cmp.GetHashCode(Option.Some(false)).Should().Be(cmp.GetHashCode(Option.Some(false)));
        cmp.GetHashCode(Option.Some(false)).Should().NotBe(cmp.GetHashCode(Option.Some(true)));
        cmp.GetHashCode(Option.Some(false)).Should().NotBe(cmp.GetHashCode(Option.Some(42)));
    }

    [TestMethod]
    public void Test_EqualityComparer_Generic_GetHashCode()
    {
        OptionEqualityComparer<bool>
            .Default.GetHashCode(Option.Some(true))
            .Should()
            .Be(OptionEqualityComparer<bool>.Default.GetHashCode(Option.Some(true)));

        OptionEqualityComparer<int>
            .Default.GetHashCode(Option.Some(42))
            .Should()
            .Be(OptionEqualityComparer<int>.Default.GetHashCode(Option.Some(42)));

        OptionEqualityComparer<float>
            .Default.GetHashCode(Option.Some(42f))
            .Should()
            .Be(OptionEqualityComparer<float>.Default.GetHashCode(Option.Some(42f)));

        OptionEqualityComparer<int>
            .Default.GetHashCode(Option.Some(69))
            .Should()
            .NotBe(OptionEqualityComparer<int>.Default.GetHashCode(Option.Some(42)));
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Operators_Equal))]
    public void Test_Option_OpEqual_True(Option lhs, Option rhs)
    {
        (lhs == rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Operators_NotEqual))]
    public void Test_Option_OpEqual_False(Option lhs, Option rhs)
    {
        (lhs == rhs).Should().BeFalse();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Operators_NotEqual))]
    public void Test_Option_OpNotEqual_True(Option lhs, Option rhs)
    {
        (lhs != rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Operators_Equal))]
    public void Test_Option_OpNotEqual_False(Option lhs, Option rhs)
    {
        (lhs != rhs).Should().BeFalse();
    }
}
