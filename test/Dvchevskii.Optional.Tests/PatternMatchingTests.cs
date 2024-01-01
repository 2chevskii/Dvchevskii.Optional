using Dvchevskii.Optional.None;
using Dvchevskii.Optional.Some;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class PatternMatchingTests
{
    private Option<int> Some;
    private Option<int> None;

    [TestInitialize]
    public void Initialize()
    {
        Some = Option.Some(42);
        None = Option.None<int>();
    }

    [TestMethod]
    public void Test_Match()
    {
        (
            Some switch
            {
                ISome => 0,
                INone => 1
            }
        ).Should().Be(0);
        (
            None switch
            {
                ISome => 0,
                INone => 1
            }
        ).Should().Be(1);
    }
}
