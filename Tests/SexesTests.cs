using EmployeeApp.API.CQRS.Queries.Sexes;
using EmployeeApp.Domain.Entities;

namespace Tests;

public class SexTests: BaseTest
{
    [Fact]
    public async Task GetSexesQueryHandler_ReturnsListOfSexes()
    {
        // Arrange
        var sex1Id = Guid.NewGuid();
        const string sex1Name = "Sex 1";
        var sex2Id = Guid.NewGuid();
        const string sex2Name = "Sex 2";
        Context.Sexes.AddRange(
            new Sex { Id = sex1Id, Name = sex1Name },
            new Sex { Id = sex2Id, Name = sex2Name }
        );
        await Context.SaveChangesAsync();

        var handler = new GetSexesQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(new GetSexesQuery(), CancellationToken.None);
        var resultAsList = result.Value?.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsError);
        Assert.Equal(2, resultAsList?.Count);
        Assert.Equal(sex1Name, resultAsList?[0].Name);
        Assert.Equal(sex2Name, resultAsList?[1].Name);
    }
}