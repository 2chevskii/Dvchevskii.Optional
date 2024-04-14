using Dvchevskii.Optional.Exceptions;
using FluentAssertions;

namespace Dvchevskii.Optional.Tests;

[TestClass]
public class ExceptionTests
{
    [TestMethod]
    public void Test_None_Unwrap_Throws()
    {
        Option.None<int>().Invoking(opt => opt.Unwrap()).Should().Throw<OptionNoneException>();
    }

    [TestMethod]
    public void Test_None_Expect_Throws()
    {
        Option
            .None<int>()
            .Invoking(opt => opt.Expect("__test__"))
            .Should()
            .Throw<OptionNoneException>()
            .WithMessage("__test__");
    }
}
