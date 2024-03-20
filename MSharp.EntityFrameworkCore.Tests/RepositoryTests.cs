using Microsoft.EntityFrameworkCore;
using MSharp.EntityFrameworkCore.Repositories.Interfaces;
using MSharp.EntityFrameworkCore.Repositories;

namespace MSharpCore.EntityFrameworkCore.Tests;
public class RepositoryTests
{
    private IExampleRepository _exampleRepository = null!;

    [Fact]
    public void Test_GetAll()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModels = _exampleRepository.GetAll();
            Assert.NotNull(exampleModels);
            Assert.NotEmpty(exampleModels);
            Assert.Equal(3, exampleModels.Count());
        });
    }
    [Fact]
    public void Test_GetSingle()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModel = _exampleRepository.GetSingle(x => x.Id == 1);
            Assert.NotNull(exampleModel);
            Assert.Equal("Test1", exampleModel!.SomeData);
            Assert.True(exampleModel.CreationDateUtc < DateTime.UtcNow);
        });

        HandleEfCoreTest(() =>
        {
            var exampleModel2 = _exampleRepository.GetSingle(x => x.SomeData == "Test3");
            Assert.NotNull(exampleModel2);
            Assert.Equal(3, exampleModel2!.Id);
            Assert.True(exampleModel2.CreationDateUtc > DateTime.UtcNow);
        });
    }
    [Fact]
    public void Test_Add()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModel = new ExampleModel { Id = 4, SomeData = "Test4", CreationDateUtc = DateTime.UtcNow.AddDays(-4) };
            _exampleRepository.Add(exampleModel);
            _exampleRepository.SaveChanges();
            var exampleModel2 = _exampleRepository.GetSingle(x => x.Id == 4);
            Assert.NotNull(exampleModel2);
            Assert.Equal("Test4", exampleModel2!.SomeData);
            Assert.True(exampleModel2.CreationDateUtc < DateTime.UtcNow);
        });
    }
    [Fact]
    public void Test_AddRange()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModels = new List<ExampleModel>
            {
                new ExampleModel { Id = 5, SomeData = "Test5", CreationDateUtc = DateTime.UtcNow.AddDays(-5) },
                new ExampleModel { Id = 6, SomeData = "Test6", CreationDateUtc = DateTime.UtcNow.AddDays(-6) }
            };
            _exampleRepository.AddRange(exampleModels);
            _exampleRepository.SaveChanges();
            var exampleModel2 = _exampleRepository.GetSingle(x => x.Id == 5);
            Assert.NotNull(exampleModel2);
            Assert.Equal("Test5", exampleModel2!.SomeData);
            Assert.True(exampleModel2.CreationDateUtc < DateTime.UtcNow);
        });
    }
    [Fact]
    public void Test_Remove()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModel = _exampleRepository.GetSingle(x => x.Id == 1);
            Assert.NotNull(exampleModel);
            _exampleRepository.Remove(exampleModel!);
            _exampleRepository.SaveChanges();
            var exampleModel2 = _exampleRepository.GetSingle(x => x.Id == 1);
            Assert.Null(exampleModel2);
        });
    }
    [Fact]
    public void Test_RemoveWhere()
    {
        HandleEfCoreTest(() =>
        {
            _exampleRepository.RemoveWhere(x => x.Id == 2);
            _exampleRepository.SaveChanges();
            var exampleModel2 = _exampleRepository.GetSingle(x => x.Id == 2);
            Assert.Null(exampleModel2);
        });
    }
    [Fact]
    public void Test_SaveChanges()
    {
        HandleEfCoreTest(() =>
        {
            var exampleModel = new ExampleModel { Id = 7, SomeData = "Test7", CreationDateUtc = DateTime.UtcNow.AddDays(-7) };
            _exampleRepository.Add(exampleModel);
            _exampleRepository.SaveChanges();
            var exampleModel2 = _exampleRepository.GetSingle(x => x.Id == 7);
            Assert.NotNull(exampleModel2);
            Assert.Equal("Test7", exampleModel2!.SomeData);
            Assert.True(exampleModel2.CreationDateUtc < DateTime.UtcNow);
        });
    }

    private void HandleEfCoreTest(Action testAction)
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: "ExampleDatabase")
            .Options;

        using (var context = new ExampleSnowflakeContext(options))
        {
            context.ExampleModels.RemoveRange(context.ExampleModels);
            context.ExampleModels.Add(new ExampleModel { Id = 1, SomeData = "Test1", CreationDateUtc = DateTime.UtcNow.AddDays(-1) });
            context.ExampleModels.Add(new ExampleModel { Id = 2, SomeData = "Test2", CreationDateUtc = DateTime.UtcNow.AddDays(-2) });
            context.ExampleModels.Add(new ExampleModel { Id = 3, SomeData = "Test3", CreationDateUtc = DateTime.UtcNow.AddDays(3) });
            context.SaveChanges();
        }

        using (var context = new ExampleSnowflakeContext(options))
        {
            _exampleRepository = new ExampleRepository(context);
            testAction();
        }
    }
    internal class ExampleModel
    {
        public int Id { get; set; }
        public string SomeData { get; set; } = null!;
        public DateTime CreationDateUtc { get; set; }
    }
    internal interface IExampleRepository : IRepository<ExampleModel> { }
    internal class ExampleRepository : Repository<ExampleModel>, IExampleRepository
    {
        public ExampleRepository(DbContext context) : base(context)
        {
        }
    }
    internal class ExampleSnowflakeContext : DbContext
    {
        public ExampleSnowflakeContext(DbContextOptions<DbContext> options) : base(options)
        {
        }
        public DbSet<ExampleModel> ExampleModels { get; set; } = null!;
    }
}