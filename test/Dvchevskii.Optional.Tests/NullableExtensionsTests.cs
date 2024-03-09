using Dvchevskii.Optional.Extensions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class NullableExtensionsTests
{
    [TestMethod]
    public void Test_AsOption()
    {
        new int?(42).AsOption().IsSomeAnd(x => x == 42).Should().BeTrue();
        new double?().AsOption().IsNone.Should().BeTrue();
        new float?(69.0f).AsOption().IsSome.Should().BeTrue();
        new bool?().AsOption().IsSome.Should().BeFalse();
    }

    private static T? AsNullable<T>(T? val)
        where T : struct
    {
        return val;
    }
}
