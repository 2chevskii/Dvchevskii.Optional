using Dvchevskii.Optional.Extensions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class NullableExtensionsTests
{
    [TestMethod]
    public void Test_NoneIfNull()
    {
        AsNullable<int>(42).NoneIfNull().IsSomeAnd(x => x == 42).Should().BeTrue();
        AsNullable<double>(null).NoneIfNull().IsNone().Should().BeTrue();
        AsNullable<float>(69.0f).NoneIfNull().IsSomeAnd(x => x == 69.0f).Should().BeTrue();
        AsNullable<bool>(null).NoneIfNull().IsNone().Should().BeTrue();
    }

    private static T? AsNullable<T>(T? val)
        where T : struct
    {
        return val;
    }
}
