var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITodoItemService, TodoItemService>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
    "/api/todo-items",
    (TodoItemCreationDto dto, ITodoItemService todoItemService)
         => todoItemService.AddTodoItem(dto.Title));

app.Run();

public record TodoItemCreationDto(string Title);
public record TodoItem(Guid Id, string Title, bool IsDone);

public interface ITodoItemService
{
    TodoItem AddTodoItem(string title);
}

internal class TodoItemService : ITodoItemService
{
    private readonly List<TodoItem> _todoItems = new();

    public TodoItem AddTodoItem(string title)
    {
        var todoItem = new TodoItem(Guid.NewGuid(), title, false);
        _todoItems.Add(todoItem);

        return todoItem;
    }
}

public partial class Program { }
