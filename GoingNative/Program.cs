using System.Data.Common;
using System.Text.Json.Serialization;
using GoingNative;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddScoped<TodoDatabase>();
builder.Services.AddScoped<DbConnection, SqliteConnection>(_ =>
    new SqliteConnection("Data Source=native.sqlite"));

var app = builder.Build();

var todosApi = app.MapGroup("/todos");

todosApi.MapGet("/", async (TodoDatabase db)
    => Results.Ok(await db.GetAll()));

todosApi.MapGet("/{id}", async (int id, TodoDatabase db) =>
    await db.GetById(id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

public record Todo(int Id,
    string? Title,
    DateOnly? DueBy = null,
    bool IsComplete = false);

[JsonSerializable(typeof(List<Todo>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}