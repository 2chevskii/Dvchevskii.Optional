using FluentAssertions;

// ReSharper disable InconsistentNaming

namespace Dvchevskii.Optional.Tests.Equality;

[TestClass]
public class OptionEqualityTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static Option[][] TestData_Some_EqualValues { get; set; }
    public static Option[][] TestData_Some_NotEqualValues { get; set; }
    public static Option[][] TestData_None { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        List<int> intList = [1, 2, 3];
        int[] intArray = [1, 2, 3];
        byte[] byteArray = [1, 2, 3];

        var dtNow = DateTime.Now;
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var dict = new Dictionary<string, int> { { "one", 1 }, { "two", 2 } };

        TestData_Some_EqualValues = new Option[][]
        {
            [Option.Some(42), Option.Some(42)],
            [Option.Some("foo"), Option.Some("foo")],
            [Option.Some(intList), Option.Some(intList)],
            [Option.Some(3.14), Option.Some(3.14)],
            [Option.Some(dtNow), Option.Some(dtNow)],
            [Option.Some(true), Option.Some(true)],
            [Option.Some('c'), Option.Some('c')],
            [Option.Some(intArray), Option.Some(intArray)],
            [Option.Some(Option.Some("nested")), Option.Some(Option.Some("nested"))],
            [Option.Some(TimeSpan.FromMinutes(5)), Option.Some(TimeSpan.FromMinutes(5))],
            [Option.Some(guid1), Option.Some(guid1)],

            [
                Option.Some(new Uri("http://example.com")),
                Option.Some(new Uri("http://example.com"))
            ],

            [
                Option.Some(dict),
                Option.Some(dict)
            ],
            [Option.Some(EnumOption.First), Option.Some(EnumOption.First)],
            [Option.Some(byteArray), Option.Some(byteArray)]
        };

        TestData_Some_NotEqualValues = new Option[][]
        {
            [Option.Some(42), Option.Some(24)],
            [Option.Some("foo"), Option.Some("bar")],
            [Option.Some(new List<int> { 1, 2 }), Option.Some(new List<int> { 1, 2, 3 })],
            [Option.Some(3.14), Option.Some(2.71)],
            [Option.Some(dtNow), Option.Some(dtNow.AddDays(1))],
            [Option.Some(true), Option.Some(false)],
            [Option.Some('c'), Option.Some('d')],
            [Option.Some(new[] { 1, 2, 3 }), Option.Some(new[] { 3, 2, 1 })],
            [Option.Some(Option.Some("nested")), Option.Some(Option.Some("different"))],
            [Option.Some(TimeSpan.FromMinutes(5)), Option.Some(TimeSpan.FromMinutes(10))],
            [Option.Some(guid1), Option.Some(guid2)],

            [
                Option.Some(new Uri("http://example.com")),
                Option.Some(new Uri("http://example.org"))
            ],

            [
                Option.Some(new Dictionary<string, int> { { "one", 1 }, { "two", 2 } }),
                Option.Some(new Dictionary<string, int> { { "one", 2 }, { "two", 1 } })
            ],
            [Option.Some(EnumOption.First), Option.Some(EnumOption.Second)],
            [Option.Some(new byte[] { 1, 2, 3 }), Option.Some(new byte[] { 3, 2, 1 })]
        };

        TestData_None = new Option[][]
        {
            [Option.None<int>(), Option.None<int>()],
            [Option.None<string>(), Option.None<string>()],
            [Option.None<object>(), Option.None<object>()],
            [Option.None<double>(), Option.None<double>()],
            [Option.None<DateTime>(), Option.None<DateTime>()],
            [Option.None<bool>(), Option.None<bool>()],
            [Option.None<char>(), Option.None<char>()],
            [Option.None<int[]>(), Option.None<int[]>()],
            [Option.None<Option<string>>(), Option.None<Option<string>>()],
            [Option.None<TimeSpan>(), Option.None<TimeSpan>()],
            [Option.None<Guid>(), Option.None<Guid>()],
            [Option.None<Uri>(), Option.None<Uri>()],
            [Option.None<Dictionary<string, int>>(), Option.None<Dictionary<string, int>>()],
            [Option.None<EnumOption>(), Option.None<EnumOption>()],
            [Option.None<byte[]>(), Option.None<byte[]>()],
            [Option.None<bool>(), Option.None<string>()]
        };
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_EqualValues))]
    public void Test_Some_OperatorEqual_ReturnsTrue(Option lhs, Option rhs)
    {
        (lhs == rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_NotEqualValues))]
    public void Test_Some_OperatorEqual_ReturnsFalse(Option lhs, Option rhs)
    {
        (lhs == rhs).Should().BeFalse();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_None))]
    public void Test_None_OperatorEqual_ReturnsTrue(Option lhs, Option rhs)
    {
        (lhs == rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_NotEqualValues))]
    public void Test_Some_OperatorNotEqual_ReturnsTrue(Option lhs, Option rhs)
    {
        (lhs != rhs).Should().BeTrue();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_Some_EqualValues))]
    public void Test_Some_OperatorNotEqual_ReturnsFalse(Option lhs, Option rhs)
    {
        (lhs != rhs).Should().BeFalse();
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_None))]
    public void Test_None_OperatorNotEqual_ReturnsFalse(Option lhs, Option rhs)
    {
        (lhs != rhs).Should().BeFalse();
    }

    private enum EnumOption
    {
        First,
        Second
    }
}

[TestClass]
public class OptionGenericEqualityTests { }
