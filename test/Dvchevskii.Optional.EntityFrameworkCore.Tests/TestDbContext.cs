using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

public class TestDbContext : DbContext
{
    public DbSet<TestEntity> Entities { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    public static TestDbContext Create() =>
        new TestDbContext(
            new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase("test").Options
        );
}