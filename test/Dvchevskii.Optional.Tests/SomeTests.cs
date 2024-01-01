using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class SomeTests
{
    private Option<int> TestSubject;

    [TestInitialize]
    public void Initialize()
    {
        TestSubject = Option.Some(42);
    }

    [TestMethod]
    public void Test_IsNone()
    {
        TestSubject.IsNone().Should().BeFalse();
    }

    [TestMethod]
    public void Test_IsSome()
    {
        TestSubject.IsSome().Should().BeTrue();
    }

    [TestMethod]
    public void Test_IsSomeAnd()
    {
        TestSubject.IsSomeAnd(x => x is 42).Should().BeTrue();
        TestSubject.IsSomeAnd(x => x is 69).Should().BeFalse();
    }

    [TestMethod]
    public void Test_Inspect()
    {
        int refValue = 0;
        TestSubject.Inspect(x => refValue = x).Should().Be(TestSubject);
        refValue.Should().Be(42);
    }

    [TestMethod]
    public void Test_Expect()
    {
        TestSubject.Invoking(s => s.Expect("__panic_msg")).Should().NotThrow();
        TestSubject.Expect("__panic_msg").Should().Be(42);
    }

    [TestMethod]
    public void Test_Unwrap()
    {
        TestSubject.Invoking(s => s.Unwrap()).Should().NotThrow();
        TestSubject.Unwrap().Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOr()
    {
        TestSubject.UnwrapOr(69).Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOrElse()
    {
        TestSubject.UnwrapOrElse(() => 69).Should().Be(42);
    }

    [TestMethod]
    public void Test_UnwrapOrDefault()
    {
        TestSubject.UnwrapOrDefault().Should().Be(42);
    }

    [TestMethod]
    public void Test_Map()
    {
        TestSubject.Map(x => x * x).Should().Be(Option.Some(42 * 42));
    }

    [TestMethod]
    public void Test_MapOr()
    {
        TestSubject.MapOr(x => x * x, 69).Should().Be(42 * 42);
    }

    [TestMethod]
    public void Test_MapOrElse()
    {
        TestSubject.MapOrElse(x => x * x, () => 69).Should().Be(42 * 42);
    }

    [TestMethod]
    public void Test_And()
    {
        TestSubject.And(Option.Some(69)).Should().Be(Option.Some(69));
    }

    [TestMethod]
    public void Test_AndThen()
    {
        TestSubject.AndThen(x => Option.Some(x * x)).Should().Be(Option.Some(42 * 42));
    }

    [TestMethod]
    public void Test_Filter()
    {
        TestSubject.Filter(x => x is 42).Should().Be(Option.Some(42));
        TestSubject.Filter(x => x is 69).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_Or()
    {
        TestSubject.Or(Option.Some(69)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_OrElse()
    {
        TestSubject.OrElse(() => Option.Some(69)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_XOr()
    {
        TestSubject.XOr(Option.None<int>()).Should().Be(Option.Some(42));
        TestSubject.XOr(Option.Some(69)).Should().Be(Option.None<int>());
        Option.None<int>().XOr(Option.Some(69)).Should().Be(Option.Some(69));
    }
}
