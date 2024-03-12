using Dvchevskii.Optional.Exceptions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class NoneTests
{
    private Option<int> _testSubject = null!;

    [TestInitialize]
    public void Initialize()
    {
        _testSubject = Option.None<int>();
    }

    [TestMethod]
    public void Test_IsNone()
    {
        _testSubject.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_IsSome()
    {
        _testSubject.IsSome.Should().BeFalse();
    }

    [TestMethod]
    public void Test_IsSomeAnd()
    {
        _testSubject.IsSomeAnd(_ => false).Should().BeFalse();
        _testSubject.IsSomeAnd(_ => true).Should().BeFalse();

        bool predicateCalled = false;

        Predicate<int> predicate = _ =>
        {
            predicateCalled = true;
            return false;
        };

        _testSubject.IsSomeAnd(predicate).Should().BeFalse();
        predicateCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_Inspect()
    {
        bool inspectCalled = false;
        _testSubject.Inspect(_ => inspectCalled = true).Should().Be(_testSubject);
        inspectCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_Expect()
    {
        string msg = "__test_msg";
        _testSubject
            .Invoking(s => s.Expect(msg))
            .Should()
            .Throw<ExpectNoneException>()
            .And.Message.Should()
            .Be(msg);
    }

    [TestMethod]
    public void Test_Unwrap()
    {
        _testSubject
            .Invoking(s => s.Unwrap())
            .Should()
            .Throw<ExpectNoneException>()
            .And.Message.Should()
            .Be("Option is none");
    }

    [TestMethod]
    public void Test_UnwrapOr()
    {
        _testSubject.Invoking(s => s.UnwrapOr(69)).Should().NotThrow();
        _testSubject.UnwrapOr(69).Should().Be(69);
    }

    [TestMethod]
    public void Test_UnwrapOrElse()
    {
        _testSubject.UnwrapOrElse(() => 69).Should().Be(69);
    }

    [TestMethod]
    public void Test_UnwrapOrDefault()
    {
        _testSubject.UnwrapOrDefault().Should().Be(default);
    }

    [TestMethod]
    public void Test_Map()
    {
        bool mapperCalled = false;
        _testSubject
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
        _testSubject
            .MapOr(_ =>
                {
                    mapperCalled = true;
                    return "";
                }, "__default_value")
            .Should()
            .Be("__default_value");
        mapperCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_MapOrElse()
    {
        bool mapperCalled = false;
        _testSubject
            .MapOrElse(_ =>
                {
                    mapperCalled = true;
                    return 42;
                }, () => 69)
            .Should()
            .Be(69);
        mapperCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Test_And()
    {
        _testSubject.And(Option.Some(42)).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_AndThen()
    {
        _testSubject.AndThen(_ => Option.Some(42)).Should().Be(Option.None<int>());
    }

    [TestMethod]
    public void Test_Filter()
    {
        bool predicateCalled = false;
        _testSubject
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
        _testSubject.Or(Option.Some(42)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_OrElse()
    {
        _testSubject.OrElse(() => Option.Some(42)).Should().Be(Option.Some(42));
    }

    [TestMethod]
    public void Test_XOr()
    {
        _testSubject.XOr(Option.Some(42)).Should().Be(Option.Some(42));
    }
}
