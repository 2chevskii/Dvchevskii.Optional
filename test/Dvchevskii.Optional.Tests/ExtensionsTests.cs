using Dvchevskii.Optional.Extensions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class ExtensionsTests
{
    [TestMethod]
    public void Test_UnwrapAll()
    {
        Option<int>[] array = { Option.Some(42), Option.Some(69), Option.None<int>() };

        array.UnwrapAll().Should().ContainInOrder(42, 69).And.HaveCount(2);
    }

    [TestMethod]
    public void Test_WrapAll_DefaultAsNone_False()
    {
        int[] array = { 42, 69, 0 };
        array.WrapAll().Should().ContainInOrder(Option.Some(42), Option.Some(69), Option.Some(0));
    }

    [TestMethod]
    public void Test_WrapAllNullAsNone_Struct()
    {
        int?[] array = [42, 69, 0];
        array
            .WrapAllNullAsNone()
            .Should()
            .ContainInOrder(Option.Some<int?>(42), Option.Some<int?>(69), Option.Some<int?>(0));
        int?[] array2 = [42, 69, null];
        array2
            .WrapAllNullAsNone()
            .Should()
            .ContainInOrder(Option.Some<int?>(42), Option.Some<int?>(69), Option.None<int?>());
    }

    [TestMethod]
    public void Test_AsSome()
    {
        42.AsSome().Should().BeAssignableTo<Option<int>>().And.Be(42);
        object? x = null;
        (x.AsSome().Equals(null)).Should().BeTrue();
        x.AsSome().Equals(null).Should().BeTrue();
    }

    [TestMethod]
    public void Test_AsNone()
    {
        Option.None<int>().Should().BeAssignableTo<Option<int>>();
        42.AsNone().Should().BeAssignableTo<Option<int>>().And.Be(Option.None<int>());
        object? x = null;
        x.AsNone().Should().BeAssignableTo<Option<object?>>().And.Be(Option.None<object?>());
        x.AsNone().Should().NotBe(new object());
        42.AsNone().Should().NotBe(42);

        x.AsNone().Should().NotBeNull();
        x.AsNone().Should().NotBe(null);
    }

    [TestMethod]
    public void Test_Flatten()
    {
        Option.Some(Option.Some(42)).Flatten().Should().Be(Option.Some(42));
        Option.Some(Option.None<string>()).Flatten().Should().Be(Option.None<string>());
        Option.None<Option<bool>>().Should().Be(Option.None<bool>());
    }

    [TestMethod]
    public void Test_Unzip()
    {
        Option.Some((1, 2)).Unzip().Should().Be((Option.Some(1), Option.Some(2)));
        Option
            .None<ValueTuple<int, int>>()
            .Unzip()
            .Should()
            .Be((Option.None<int>(), Option.None<int>()));
    }

    [TestMethod]
    public void Test_ParseBool()
    {
        "True".ToBoolOption().Should().Be(Option.Some(true)).And.Be(true);
        "false".ToBoolOption().Should().Be(Option.Some(false)).And.Be(false);
        "NotTrueOrFalse".ToBoolOption().Should().Be(Option.None<bool>());
    }

    /*
     * TODO: Write test for all the "To<type>Option" parsing methods
     */
}
