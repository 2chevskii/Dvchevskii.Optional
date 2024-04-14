using FluentAssertions;

namespace Dvchevskii.Optional.Tests.Equality;

[TestClass]
public class OptionEqualityComparerGenericTests
{
    [TestMethod]
    public void Test_Some_EqualValues_ReturnsTrue()
    {
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42), Option.Some(42), true);
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42.0d), Option.Some(42.0d), true);
        CallEqualityComparer_Generic_ReturnsX(Option.Some("value"), Option.Some("value"), true);
        CallEqualityComparer_Generic_ReturnsX(Option.Some(false), Option.Some(false), true);
        var objRef = new object();
        CallEqualityComparer_Generic_ReturnsX(Option.Some(objRef), Option.Some(objRef), true);
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<object?>(null),
            Option.Some<object?>(null),
            true
        );
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<string?>(null),
            Option.Some<string?>(null),
            true
        );
    }

    [TestMethod]
    public void Test_Some_DifferentValues_ReturnsFalse()
    {
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42), Option.Some(69), false);
        CallEqualityComparer_Generic_ReturnsX(Option.Some(true), Option.Some(false), false);
        CallEqualityComparer_Generic_ReturnsX(Option.Some("value1"), Option.Some("value2"), false);
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42.0f), Option.Some(69.0f), false);
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some(new object()),
            Option.Some(new object()),
            false
        );
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<object?>(null),
            Option.Some<object?>(new object()),
            false
        );
    }

    [TestMethod]
    public void Test_None_ReturnsTrue()
    {
        CallEqualityComparer_Generic_ReturnsX(Option.None<int>(), Option.None<int>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<int?>(), Option.None<int?>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<bool>(), Option.None<bool>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<float>(), Option.None<float>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<double>(), Option.None<double>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<string>(), Option.None<string>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<string?>(), Option.None<string?>(), true);
        CallEqualityComparer_Generic_ReturnsX(Option.None<object>(), Option.None<object>(), true);
    }

    [TestMethod]
    public void Test_Some_None_ReturnsFalse()
    {
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42), Option.None<int>(), false);
        CallEqualityComparer_Generic_ReturnsX(Option.Some(42.0f), Option.None<float>(), false);
        CallEqualityComparer_Generic_ReturnsX(Option.Some("test"), Option.None<string>(), false);
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<string?>(null),
            Option.None<string?>(),
            false
        );
        CallEqualityComparer_Generic_ReturnsX(Option.Some(true), Option.None<bool>(), false);
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<bool?>(false),
            Option.None<bool?>(),
            false
        );
        CallEqualityComparer_Generic_ReturnsX(
            Option.Some<bool?>(null),
            Option.None<bool?>(),
            false
        );
    }

    private void CallEqualityComparer_Generic_ReturnsX<T>(
        Option<T> lhs,
        Option<T> rhs,
        bool expected
    )
    {
        new OptionEqualityComparer<T>().Equals(lhs, rhs).Should().Be(expected);
    }
}
