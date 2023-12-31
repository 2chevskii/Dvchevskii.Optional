using FluentAssertions;
using Option;

namespace Option.Tests;

[TestClass]
public class OptionStaticTests
{
    [TestMethod]
    public void TestCreateNone()
    {
        var none = Option.None<object>();
        var none2 = Option.None<int>();
        var notNone = Option.Some(0);
        
        none.Should().Match<Option<object>>(x => x.IsNone());
        none2.Should().Match<Option<int>>(x => x.IsNone());
        none2.Should().Match<Option<int>>(x => x.IsSome() == false);
        notNone.Should().Match<Option<int>>(x => x.IsNone() == false);
        notNone.Should().Match<Option<int>>(x => x.IsSome() == true);
    }
}