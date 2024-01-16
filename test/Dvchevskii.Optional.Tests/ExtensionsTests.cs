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
        array
            .WrapAll(defaultAsNone: false)
            .Should()
            .ContainInOrder(Option.Some(42), Option.Some(69), Option.Some(0));
    }

    [TestMethod]
    public void Test_WrapAll_DefaultAsNone_True()
    {
        int[] array = { 42, 69, 0 };
        array
            .WrapAll(defaultAsNone: true)
            .Should()
            .ContainInOrder(Option.Some(42), Option.Some(69), Option.None<int>());
    }

    [TestMethod]
    public void Test_ToOption()
    {
        42.ToOption().Should().BeAssignableTo<Option<int>>().And.Be(42);
        "somestring".ToOption().Should().BeAssignableTo<Option<string>>().And.Be("somestring");
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
        "True".ToBoolOption().Should().Be(Option.Some(true));
        "false".ToBoolOption().Should().Be(Option.Some(false));
        "NotTrueOrFalse".ToBoolOption().Should().Be(Option.None<bool>());
    }

    /*
     * TODO: Write test for all the "To<type>Option" parsing methods
     */
}
