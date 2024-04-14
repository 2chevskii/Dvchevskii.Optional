using Dvchevskii.Optional.Async;
using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class DbContextExtensionsTests
{
    [TestMethod]
    public void Test_FindOrNone()
    {
        TestDbContext dbContext = TestDbContext.Create();
        Guid entId = Guid.NewGuid();
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
        dbContext.FindOrNone<TestEntity>(Guid.NewGuid()).IsSome.Should().BeFalse();
        dbContext.FindOrNone(typeof(TestEntity), Guid.NewGuid()).IsSome.Should().BeFalse();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync()
    {
        TestDbContext dbContext = TestDbContext.Create();
        Guid entId = Guid.NewGuid();
        dbContext.Add(
            new TestEntity
            {
                Id = entId,
                Name = "Test name",
                CreatedAt = DateTimeOffset.Now
            }
        );
        await dbContext.SaveChangesAsync();

        Option<TestEntity> findOrNoneTResult = await dbContext.FindOrNoneAsync<TestEntity>(entId);
        Option<object> findOrNoneResult = await dbContext.FindOrNoneAsync(
            typeof(TestEntity),
            entId
        );

        Option<TestEntity> findOrNoneTFalseResult = await dbContext.FindOrNoneAsync<TestEntity>(
            Guid.NewGuid()
        );
        Option<object> findOrNoneFalseResult = await dbContext.FindOrNoneAsync(
            typeof(TestEntity),
            Guid.NewGuid()
        );

        findOrNoneTResult.IsSome.Should().BeTrue();
        findOrNoneResult.IsSome.Should().BeTrue();
        findOrNoneTFalseResult.IsSome.Should().BeFalse();
        findOrNoneFalseResult.IsSome.Should().BeFalse();
    }
}
