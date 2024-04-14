using FluentAssertions;

// ReSharper disable InconsistentNaming

namespace Dvchevskii.Optional.Tests.Equality;

[TestClass]
public class OptionEqualityComparerTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static Option[][] TestData_Some_EqualValues { get; set; }
    public static Option[][] TestData_Some_DifferentValues { get; set; }
    public static Option[][] TestData_None_SameType { get; set; }
    public static Option[][] TestData_None_DifferentTypes { get; set; }
    public static Option[][] TestData_Some_None_SameType { get; set; }
    public static Option[][] TestData_Some_None_DifferentTypes { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        object objRef = new();

        TestData_Some_EqualValues =
        [
            [Option.Some(42), Option.Some(42)],
            [Option.Some(42.0f), Option.Some(42.0f)],
            [Option.Some(42.0d), Option.Some(42.0d)],
            [Option.Some(42.0m), Option.Some(42.0m)],
            [Option.Some("value"), Option.Some("value")],
            [Option.Some<string?>(null), Option.Some<string?>(null)],
            [Option.Some(true), Option.Some(true)],
            [Option.Some(false), Option.Some(false)],
            [Option.Some(objRef), Option.Some(objRef)],
            [Option.Some<object?>(null), Option.Some<object?>(null)],
            [Option.Some<int?>(42), Option.Some<int?>(42)],
            [Option.Some<int?>(null), Option.Some<int?>(null)],
            [Option.Some<bool?>(true), Option.Some<bool?>(true)],
            [Option.Some<bool?>(null), Option.Some<bool?>(null)]
        ];

        object objRef1 = new();
        object objRef2 = new();

        TestData_Some_DifferentValues =
        [
            [Option.Some(42), Option.Some(69)],
            [Option.Some(true), Option.Some(false)],
            [Option.Some(objRef1), Option.Some(objRef2)],
            [Option.Some(42), Option.Some(42.0f)]
        ];

        TestData_None_SameType =
        [
            [Option.None<int>(), Option.None<int>()],
            [Option.None<bool>(), Option.None<bool>()],
            [Option.None<float>(), Option.None<float>()],
            [Option.None<float?>(), Option.None<float?>()],
            [Option.None<string>(), Option.None<string>()],
            [Option.None<string?>(), Option.None<string?>()],
            [Option.None<object?>(), Option.None<object?>()],
        ];

        TestData_None_DifferentTypes =
        [
            [Option.None<int>(), Option.None<float>()],
            [Option.None<int>(), Option.None<int?>()],
            [Option.None<int>(), Option.None<bool>()],
            [Option.None<int>(), Option.None<string>()],
            [Option.None<string>(), Option.None<object>()],
            [Option.None<string>(), Option.None<double?>()],
            [Option.None<object>(), Option.None<double?>()],
        ];

        TestData_Some_None_SameType =
        [
            [Option.Some(42), Option.None<int>()],
            [Option.Some(true), Option.None<bool>()],
            [Option.Some(false), Option.None<bool>()],
            [Option.Some<string?>(null), Option.None<string?>()],
            [Option.Some("test"), Option.None<string>()],
            [Option.Some<int?>(null), Option.None<int?>()],
        ];

        TestData_Some_None_DifferentTypes =
        [
            [Option.Some(42), Option.None<float>()],
            [Option.Some("test"), Option.None<object>()],
            [Option.Some<string?>(null), Option.None<double>()],
            [Option.Some<string?>(null), Option.None<string>()],
            [Option.Some(42.0d), Option.None<float>()],
        ];
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_EqualValues))]
    public void Test_Some_EqualValues_ReturnsTrue(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_DifferentValues))]
    public void Test_Some_DifferentValues_ReturnsFalse(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeFalse();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_None_SameType))]
    public void Test_None_SameType_ReturnsTrue(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_None_DifferentTypes))]
    public void Test_None_DifferentTypes_ReturnsTrue(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_None_SameType))]
    public void Test_Some_None_SameType_ReturnsFalse(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeFalse();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_None_DifferentTypes))]
    public void Test_Some_None_DifferentTypes_ReturnsFalse(Option lhs, Option rhs)
    {
        OptionEqualityComparer comparer = new();

        comparer.Equals(lhs, rhs).Should().BeFalse();
    }
}
