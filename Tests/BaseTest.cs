using AutoMapper;
using EmployeeApp.API.Mapper;
using EmployeeApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class BaseTest : IDisposable, IAsyncDisposable
{
    protected readonly EmployeeDbContext Context;
    protected readonly IMapper Mapper;

    protected BaseTest()
    {
        var options = new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new EmployeeDbContext(options);
        Context.Database.EnsureCreated();
        Mapper = AutoMapperModule.CreateAutoMapper();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Context.DisposeAsync();
    }
}