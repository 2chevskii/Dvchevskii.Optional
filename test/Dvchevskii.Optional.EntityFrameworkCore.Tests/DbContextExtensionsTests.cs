using FluentAssertions;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

[TestClass]
public class DbContextExtensionsTests
{
    [TestMethod]
    public void Test_FindOrNone_Object_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        Option<object>? option = dbContext.FindOrNone(typeof(TestEntity), 1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Object_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        Option<object>? option = await dbContext.FindOrNoneAsync(typeof(TestEntity), 1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FindOrNone_Generic_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        Option<TestEntity>? option = dbContext.FindOrNone<TestEntity>(1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Generic_Empty_DbContext_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateEmpty();

        Option<TestEntity>? option = await dbContext.FindOrNoneAsync<TestEntity>(1);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FindOrNone_Object_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<object>? option = dbContext.FindOrNone(typeof(TestEntity), 999);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FindOrNone_Generic_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity>? option = dbContext.FindOrNone<TestEntity>(999);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Object_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<object>? option = await dbContext.FindOrNoneAsync(typeof(TestEntity), 999);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FindOrNoneAsync_Generic_Data_DbContext_Wrong_Key_Returns_None()
    {
        TestDbContext dbContext = TestDbContext.CreateWithData();

        Option<TestEntity>? option = await dbContext.FindOrNoneAsync<TestEntity>(999);
        option.IsNone.Should().BeTrue();
    }
}
