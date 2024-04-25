using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class QueryableExtensionsTests
{
    private static readonly TestDbContext EmptyDbContext = TestDbContext.CreateEmpty();
    private static readonly TestDbContext DataDbContext = TestDbContext.CreateWithData();

    [TestMethod]
    public void Test_FirstOrNone_Empty_DbContext_Returns_None()
    {
        EmptyDbContext.Entities.AsQueryable().FirstOrNone().IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FirstOrNone_Predicate_Empty_DbContext_Returns_None()
    {
        EmptyDbContext.Entities.AsQueryable().FirstOrNone(x => x.Id == 1).IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FirstOrNoneAsync_Empty_DbContext_Returns_None()
    {
        Option<TestEntity> option = await EmptyDbContext.Entities.AsQueryable().FirstOrNoneAsync();
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FirstOrNoneAsync_Predicate_Empty_DbContext_Returns_None()
    {
        var option = await EmptyDbContext.Entities.AsQueryable().FirstOrNoneAsync(x => x.Id == 1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FirstOrNone_Data_DbContext_Returns_Some()
    {
        DataDbContext.Entities.AsQueryable().FirstOrNone().IsSome.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FirstOrNone_Predicate_Data_DbContext_Returns_Some()
    {
        DataDbContext.Entities.AsQueryable().FirstOrNone(x => x.Id == 1).IsSome.Should().BeTrue();
        DataDbContext.Entities.AsQueryable().FirstOrNone(x => x.Id == 1).Unwrap().Id.Should().Be(1);
        DataDbContext.Entities.AsQueryable().FirstOrNone(x => x.Name == "entity42").IsSome.Should().BeTrue();
        DataDbContext.Entities.AsQueryable().FirstOrNone(x => x.Name == "entity42").Unwrap().Name.Should().Be("entity42");
    }
}
