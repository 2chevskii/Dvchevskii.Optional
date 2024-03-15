using System.Reflection.Metadata;
using FluentAssertions;

namespace Dvchevskii.Optional.Async.Tests;

[TestClass]
public class AsyncOptionTests
{
    [TestMethod]
    public async Task Test_AwaitSome()
    {
        var option = await ReturnsSome();
        option.IsSome.Should().BeTrue();
        option.UnwrapOr(69).Should().Be(42);
    }

    [TestMethod]
    public async Task Test_AwaitNone()
    {
        var option = await ReturnsNone();
        option.IsNone.Should().BeTrue();
        option.UnwrapOr(69).Should().Be(69);
    }

    [DataTestMethod]
    [DynamicData(nameof(TestData_AsyncMapping))]
    public async Task Test_AsyncMapping(Task<Option<int>> task, int defaultValue, int expectedValue)
    {
        var value = await task.AsAsyncOption().MapOrAsync(v => v, defaultValue);
        value.Should().Be(expectedValue);
    }

    private AsyncOption<int> ReturnsSome() => Task.FromResult(Option.Some(42)).AsAsyncOption();
    private AsyncOption<int> ReturnsNone() => Task.FromResult(Option.None<int>()).AsAsyncOption();

    public static IEnumerable<object[]> TestData_AsyncMapping =>
        new object[][]
        {
            new object[] { Task.FromResult(Option.Some(42)), 11, 42 },
            new object[] { Task.FromResult(Option.Some(69)), 128, 69 },
            new object[] { Task.FromResult(Option.None<int>()), 111, 111 },
            new object[] { Task.FromResult(Option.None<int>()), 12, 12 },
        };
}
