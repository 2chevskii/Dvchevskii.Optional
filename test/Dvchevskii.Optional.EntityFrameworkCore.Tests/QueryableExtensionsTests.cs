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

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(10)]
    [DataRow(42)]
    public void Test_FirstOrNone_Predicate_Empty_DbContext_Returns_None(int id)
    {
        EmptyDbContext.Entities.AsQueryable().FirstOrNone(x => x.Id == id).IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task Test_FirstOrNoneAsync_Empty_DbContext_Returns_None()
    {
        Option<TestEntity> option = await EmptyDbContext.Entities.AsQueryable().FirstOrNoneAsync();
        option.IsNone.Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(10)]
    [DataRow(42)]
    public async Task Test_FirstOrNoneAsync_Predicate_Empty_DbContext_Returns_None(int id)
    {
        var option = await EmptyDbContext.Entities.AsQueryable().FirstOrNoneAsync(x => x.Id == id);
        option.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Test_FirstOrNone_Data_DbContext_Returns_Some()
    {
        DataDbContext.Entities.AsQueryable().FirstOrNone().IsSome.Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(42)]
    public void Test_FirstOrNone_Predicate_Id_Data_DbContext_Returns_Some(int id)
    {
        DataDbContext
            .Entities.AsQueryable()
            .FirstOrNone(x => x.Id == id)
            .Should()
            .Match<Option<TestEntity>>(x => x.IsSome)
            .And.Subject.As<Option<TestEntity>>()
            .Unwrap()
            .Id.Should()
            .Be(id);
    }

    [DataTestMethod]
    [DataRow("entity1")]
    [DataRow("entity42")]
    public void Test_FirstOrNone_Predicate_Name_Data_DbContext_Returns_Some(string name)
    {
        DataDbContext
            .Entities.AsQueryable()
            .FirstOrNone(x => x.Name == name)
            .Should()
            .Match<Option<TestEntity>>(x => x.IsSome)
            .And.Subject.As<Option<TestEntity>>()
            .Unwrap()
            .Name.Should()
            .Be(name);
    }
}
