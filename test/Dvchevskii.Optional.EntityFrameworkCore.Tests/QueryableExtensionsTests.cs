using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class QueryableExtensionsTests
{
    [TestMethod]
    public void Test_FirstOrNone_SingleOrNone_ReturnsNone()
    {
        TestDbContext dbContext = TestDbContext.Create();
        dbContext.Entities.AsQueryable().FirstOrNone().Should().Match<Option<TestEntity>>(x => x.IsNone);
        dbContext.Entities.AsQueryable().SingleOrNone().Should().Match<Option<TestEntity>>(x => x.IsNone);
    }
}
