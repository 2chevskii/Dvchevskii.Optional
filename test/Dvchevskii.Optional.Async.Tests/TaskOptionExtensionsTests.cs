using Dvchevskii.Optional.Exceptions;
using FluentAssertions;

namespace Dvchevskii.Optional.Async.Tests;

[TestClass]
public class TaskOptionExtensionsTests
{
    [TestMethod]
    public async Task Test_AsyncBasic()
    {
        (await Task.FromResult(Option.Some(42))).Unwrap().Should().Be(42);
        (await Task.FromResult(Option.Some(69))).Unwrap().Should().Be(69);
        (await Task.Run(() => Option.Some(42))).Unwrap().Should().Be(42);
        (await Task.Run(() => Option.Some(69))).Unwrap().Should().Be(69);

        var t1 = new Task<Option<int>>(() => Option.Some(42));
        t1.Start();
        var t2 = new Task<Option<int>>(() => Option.Some(69));
        t2.Start();

        (await t1).Unwrap().Should().Be(42);
        (await t2).Unwrap().Should().Be(69);
    }

    [TestMethod]
    public async Task Test_IsSomeAsyncT()
    {
        var t1 = new Task<Option<int>>(() => Option.Some(42));
        t1.Start();
        (await t1.IsSomeAsync()).Should().BeTrue();
        (await Task.FromResult(Option.Some(42)).IsSomeAsync()).Should().BeTrue();
        (await Task.Run(() => Option.Some(42)).IsSomeAsync()).Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_IsNoneAsyncT()
    {
        var t1 = new Task<Option<int>>(Option.None<int>);
        t1.Start();
        (await t1.IsNoneAsync()).Should().BeTrue();
        (await Task.FromResult(Option.None<int>()).IsNoneAsync()).Should().BeTrue();
        (await Task.Run(Option.None<int>).IsNoneAsync()).Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_UnwrapAsync()
    {
        (await Task.FromResult(Option.Some(42)).UnwrapAsync()).Should().Be(42);
        await Task.FromResult(Option.None<int>())
            .Awaiting((Func<Task<Option<int>>, ValueTask>)(async t => await t.UnwrapAsync()))
            .Should()
            .ThrowAsync<ExpectNoneException>();
    }

    [TestMethod]
    public async Task Test_UnwrapOrAsync()
    {
        (await Task.FromResult(Option.Some(42)).UnwrapOrAsync(69)).Should().Be(42);
        (await Task.FromResult(Option.None<int>()).UnwrapOrAsync(69)).Should().Be(69);
    }
}
