using FluentAssertions;

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class PatternMatchingTests
{
    private Option<int> _some = null!;
    private Option<int> _none = null!;

    [TestInitialize]
    public void Initialize()
    {
        _some = Option.Some(42);
        _none = Option.None<int>();
    }

    [TestMethod]
    public void Test_Match()
    {
        (
            _some switch
            {
                Some<int> => 0,
                None<int> => 1
            }
        ).Should().Be(0);
        (
            _none switch
            {
                Some<int> => 0,
                None<int> => 1
            }
        ).Should().Be(1);
    }
}
