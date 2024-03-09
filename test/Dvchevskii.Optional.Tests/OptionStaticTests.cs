using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class OptionStaticTests
{
    [TestMethod]
    public void TestCreateNone()
    {
        Option<object> none = Option.None<object>();
        Option<int> none2 = Option.None<int>();
        Option<int> notNone = Option.Some(0);

        none.Should().Match<Option<object>>(x => x.IsNone);
        none2.Should().Match<Option<int>>(x => x.IsNone);
        none2.Should().Match<Option<int>>(x => x.IsSome == false);
        notNone.Should().Match<Option<int>>(x => x.IsNone == false);
        notNone.Should().Match<Option<int>>(x => x.IsSome == true);
    }
}
