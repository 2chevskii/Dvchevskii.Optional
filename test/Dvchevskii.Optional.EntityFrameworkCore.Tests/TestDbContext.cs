using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCore.Tests;

public class TestDbContext : DbContext
{
    public DbSet<TestEntity> Entities { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public static TestDbContext CreateEmpty(string name = "empty")
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(name);

        return new TestDbContext(optionsBuilder.Options);
    }

    public static TestDbContext CreateWithData(string name = "data")
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(name);

        var dbContext = new TestDbContext(optionsBuilder.Options);

        if (!dbContext.Entities.Any())
        {
            dbContext.AddRange(
                new TestEntity
                {
                    Id = 1,
                    Name = "entity1",
                    CreatedAt = DateTimeOffset.UnixEpoch
                },
                new TestEntity
                {
                    Id = 42,
                    Name = "entity42",
                    CreatedAt = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
                }
            );
            dbContext.SaveChanges();
        }


        return dbContext;
    }
}
