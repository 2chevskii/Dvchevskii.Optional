using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class DbSetExtensionsTests
{
    [TestMethod]
    public async Task Test()
    {
        TestDbContext dbContext = TestDbContext.Create();
        dbContext.AddRange(
            [
                new TestEntity { Name = "name1" },
                new TestEntity { Name = "name2" },
                new TestEntity { Name = "name3" }
            ]
        );
        await dbContext.SaveChangesAsync();

        Option<TestEntity> res = await dbContext.Entities.FirstOrNoneAsync(x => x.Name == "name3");
        Option<TestEntity> res2 = await dbContext.Entities.FirstOrNoneAsync(
            x => x.Name == "name4"
        );

        res.IsSome.Should().BeTrue();
        res.Unwrap().Name.Should().Be("name3");
        res2.IsSome.Should().BeFalse();
    }
}
