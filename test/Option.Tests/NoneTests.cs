using FluentAssertions;
using Option.Exceptions;

namespace Option.Tests;

[TestClass]
public class NoneTests
{
    private Option<int> TestSubject;

    [TestInitialize]
    public void Initialize()
    {
        TestSubject = Option.None<int>();
    }

    [TestMethod]
    public void Test_IsNone()
    {
        TestSubject.IsNone().Should().BeTrue();
    }

    [TestMethod]
    public void Test_IsSome()
    {
        TestSubject.IsSome().Should().BeFalse();
    }

    [TestMethod]
    public void Test_IsSomeAnd()
    {
        TestSubject.IsSomeAnd(_ => false).Should().BeFalse();
        TestSubject.IsSomeAnd(_ => true).Should().BeFalse();

        bool predicateCalled = false;

        Predicate<int> predicate = _ =>
        {
            predicateCalled = true;
            return false;
        };

        TestSubject.IsSomeAnd(predicate).Should().BeFalse();
        predicateCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_Inspect()
    {
        bool inspectCalled = false;
        TestSubject.Inspect(_ => inspectCalled = true).Should().Be(TestSubject);
        inspectCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_Expect()
    {
        string msg = "__test_msg";
        TestSubject
            .Invoking(s => s.Expect(msg))
            .Should()
            .Throw<NoneOptionException>()
            .And.Message.Should()
            .Be(msg);
    }

    [TestMethod]
    public void Test_Unwrap()
    {
        TestSubject
            .Invoking(s => s.Unwrap())
            .Should()
            .Throw<NoneOptionException>()
            .And.Message.Should()
            .Be("Option is none");
    }

    [TestMethod]
    public void Test_UnwrapOr()
    {
        TestSubject.Invoking(s => s.UnwrapOr(69)).Should().NotThrow();
        TestSubject.UnwrapOr(69).Should().Be(69);
    }

    [TestMethod]
    public void Test_UnwrapOrElse()
    {
        TestSubject.UnwrapOrElse(() => 69).Should().Be(69);
    }

    [TestMethod]
    public void Test_UnwrapOrDefault()
    {
        TestSubject.UnwrapOrDefault().Should().Be(default);
    }

    [TestMethod]
    public void Test_Map()
    {
        bool mapperCalled = false;
        TestSubject
            .Map(_ =>
            {
                mapperCalled = true;
                return "";
            })
            .Should()
            .Be(Option.None<string>());
        mapperCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_MapOr()
    {
        bool mapperCalled = false;
        TestSubject
            .MapOr(
                "__default_value",
                _ =>
                {
                    mapperCalled = true;
                    return "";
                }
            )
            .Should()
            .Be("__default_value");
        mapperCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_MapOrElse()
    {
        bool mapperCalled = false;
        TestSubject
            .MapOrElse(
                () => 69,
                _ =>
                {
                    mapperCalled = true;
                    return 42;
                }
            )
            .Should()
            .Be(69);
        mapperCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_And()
    {
        TestSubject.And(Option.Some(42)).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_AndThen()
    {
        TestSubject.AndThen(_ => Option.Some(42)).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_Filter()
    {
        bool predicateCalled = false;
        TestSubject
            .Filter(x =>
            {
                predicateCalled = true;
                return false;
            })
            .Should()
            .Be(Option.None<int>());
        predicateCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_Or()
    {
        TestSubject.Or(Option.Some(42)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_OrElse()
    {
        TestSubject.OrElse(() => Option.Some(42)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_XOr()
    {
        TestSubject.XOr(Option.Some(42)).Should().Be(Option.Some(42));
    }
}
