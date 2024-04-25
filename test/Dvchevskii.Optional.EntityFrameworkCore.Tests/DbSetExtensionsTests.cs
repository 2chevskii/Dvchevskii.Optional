using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass, TestCategory("DbSetExtensions")]
public class DbSetExtensionsTests
{
    [TestMethod]
    public void Test_FindOrNone_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        Option<TestEntity> option = dbContext.Entities.FindOrNone(1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        var option = await dbContext.Entities.FindOrNoneAsync(1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FindOrNone_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity> option = dbContext.Entities.FindOrNone(999);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity> option = await dbContext.Entities.FindOrNoneAsync(999);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FindOrNone_Data_DbContext_Returns_Some()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity> option = dbContext.Entities.FindOrNone(1);
        option.IsSome.Should().BeTrue();
        option.IsSomeAnd(x => x.Id == 1).Should().BeTrue();
        option.Unwrap().Name.Should().Be("entity1");
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Data_DbContext_Returns_Some()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity> option = await dbContext.Entities.FindOrNoneAsync(1);
        option.IsSome.Should().BeTrue();
        option.IsSomeAnd(x => x.Id == 1).Should().BeTrue();
        option.Unwrap().Name.Should().Be("entity1");
    }
}
