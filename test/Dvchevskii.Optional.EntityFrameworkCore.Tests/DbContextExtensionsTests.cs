using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class DbContextExtensionsTests
{
    [TestMethod]
    public void Test_FindOrNone()
    {
        var dbContext = TestDbContext.Create();
        var entId = Guid.NewGuid();
        dbContext.Add(
            new TestEntity
            {
                Id = entId,
                Name = "Test name",
                CreatedAt = DateTimeOffset.Now
            }
        );
        dbContext.SaveChanges();

        dbContext.FindOrNone<TestEntity>(entId).IsSome.Should().BeTrue();
        dbContext.FindOrNone(typeof(TestEntity), entId).IsSome.Should().BeTrue();
        dbContext.FindOrNone<TestEntity>(Guid.NewGuid()).IsNone.Should().BeTrue();
        dbContext.FindOrNone(typeof(TestEntity), Guid.NewGuid()).IsNone.Should().BeTrue();
    }
}
