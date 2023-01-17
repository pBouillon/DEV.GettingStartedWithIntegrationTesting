using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using System.Net;
using System.Net.Http.Json;

using Xunit;

namespace Api.IntegrationTests;

internal class TestTodoItemService : ITodoItemService
{
    public TodoItem AddTodoItem(string title)
        => new(Guid.Empty, "test", false);
}

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
        => Client = factory
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.AddScoped<ITodoItemService, TestTodoItemService>();
            }))
            .CreateClient();
}

public class TodoItemCreationEndpointTest : IntegrationTestBase
{
    public TodoItemCreationEndpointTest(WebApplicationFactory<Program> factory)
        : base(factory) { }

    [Fact]
    public async Task CreatingATodoItemShouldReturnIt()
    {
        // Arrange
        var payload = new TodoItemCreationDto("My todo");

        // Act
        var result = await Client.PostAsJsonAsync("/api/todo-items", payload);
        var content = await result.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("test", content?.Title);
        Assert.False(content?.IsDone);
    }
}
