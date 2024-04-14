using FluentAssertions;

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class PatternMatchingTests
{
    [TestMethod]
    public void Test_SwitchExpression()
    {
        bool someMatched = Option.Some(42) switch
        {
            Some<int> => true,
            None<int> => false
        };
        bool noneMatched = Option.None<int>() switch
        {
            Some<int> => true,
            None<int> => false
        };

        someMatched.Should().BeTrue();
        noneMatched.Should().BeFalse();
    }
}
