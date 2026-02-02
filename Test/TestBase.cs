using Datos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Test;

public abstract class TestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly IncapacidadesContext Contexto;

    protected TestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<IncapacidadesContext>()
            .UseSqlite(_connection)
            .Options;

        Contexto = new IncapacidadesContext(options);
        Contexto.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Contexto.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
