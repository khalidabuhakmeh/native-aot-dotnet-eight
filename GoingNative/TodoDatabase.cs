using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace GoingNative;

public class TodoDatabase: IDisposable, IAsyncDisposable
{
    private readonly DbConnection _connection;

    public TodoDatabase(DbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Todo?> GetById(int id)
    {
        await _connection.OpenAsync();
        var command = _connection.CreateCommand();
        command.CommandText = "select Id, Title, DueBy, IsComplete from Todos Where Id = @Id";
        command.Parameters.Add(new SqliteParameter("@Id", id));

        var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        await reader.ReadAsync();

        return new Todo(
            reader.GetInt32(0),
            reader.GetString(1),
            DateOnly.FromDateTime(DateTime.Parse(reader.GetString(2))),
            reader.GetBoolean(3));
    }

    public async Task<List<Todo>> GetAll()
    {
        await _connection.OpenAsync();
        var command = _connection.CreateCommand();
        command.CommandText = "select Id, Title, DueBy, IsComplete from Todos";
        var reader = await command.ExecuteReaderAsync();
        List<Todo> results = new();
        while (await reader.ReadAsync())
        {
            results.Add(new Todo(
                reader.GetInt32(0),
                reader.GetString(1),
                DateOnly.FromDateTime(DateTime.Parse(reader.GetString(2))),
                reader.GetBoolean(3))
            );
        }

        return results;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}