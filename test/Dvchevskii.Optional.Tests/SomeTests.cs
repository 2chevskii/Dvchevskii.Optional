using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class SomeTests
{
    private Option<int> _testSubject = null!;

    [TestInitialize]
    public void Initialize()
    {
        _testSubject = Option.Some(42);
    }

    [TestMethod]
    public void Test_IsNone()
    {
        _testSubject.IsNone.Should().BeFalse();
    }

    [TestMethod]
    public void Test_IsSome()
    {
        _testSubject.IsSome.Should().BeTrue();
    }

    [TestMethod]
    public void Test_IsSomeAnd()
    {
        _testSubject.IsSomeAnd(x => x is 42).Should().BeTrue();
        _testSubject.IsSomeAnd(x => x is 69).Should().BeFalse();
    }

    [TestMethod]
    public void Test_Inspect()
    {
        int refValue = 0;
        _testSubject.Inspect(x => refValue = x).Should().Be(_testSubject);
        refValue.Should().Be(42);
    }

    [TestMethod]
    public void Test_Expect()
    {
        _testSubject.Invoking(s => s.Expect("__panic_msg")).Should().NotThrow();
        _testSubject.Expect("__panic_msg").Should().Be(42);
    }

    [TestMethod]
    public void Test_Unwrap()
    {
        _testSubject.Invoking(s => s.Unwrap()).Should().NotThrow();
        _testSubject.Unwrap().Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOr()
    {
        _testSubject.UnwrapOr(69).Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOrElse()
    {
        _testSubject.UnwrapOrElse(() => 69).Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOrDefault()
    {
        _testSubject.UnwrapOrDefault().Should().Be(42);
    }

    [TestMethod]
    public void Test_Map()
    {
        _testSubject.Map(x => x * x).Should().Be(Option.Some(42 * 42));
    }

    [TestMethod]
    public void Test_MapOr()
    {
        _testSubject.MapOr(x => x * x, 69).Should().Be(42 * 42);
    }

    [TestMethod]
    public void Test_MapOrElse()
    {
        _testSubject.MapOrElse(x => x * x, () => 69).Should().Be(42 * 42);
    }

    [TestMethod]
    public void Test_And()
    {
        _testSubject.And(Option.Some(69)).Should().Be(Option.Some(69));
    }

    [TestMethod]
    public void Test_AndThen()
    {
        _testSubject.AndThen(x => Option.Some(x * x)).Should().Be(Option.Some(42 * 42));
    }

    [TestMethod]
    public void Test_Filter()
    {
        _testSubject.Filter(x => x is 42).Should().Be(Option.Some(42));
        _testSubject.Filter(x => x is 69).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_Or()
    {
        _testSubject.Or(Option.Some(69)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_OrElse()
    {
        _testSubject.OrElse(() => Option.Some(69)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_XOr()
    {
        _testSubject.XOr(Option.None<int>()).Should().Be(Option.Some(42));
        _testSubject.XOr(Option.Some(69)).Should().Be(Option.None<int>());
        Option.None<int>().XOr(Option.Some(69)).Should().Be(Option.Some(69));
    }
}
