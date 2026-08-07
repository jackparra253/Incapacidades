using Api;
using Bitakora.Persistencia;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Test.Api;

public class ApiDePrueba : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _conexion;

    public ApiDePrueba()
    {
        _conexion = new SqliteConnection("DataSource=:memory:");
        _conexion.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");

        builder.ConfigureServices(servicios =>
        {
            servicios.RemoveAll<DbContextOptions<IncapacidadesContext>>();
            servicios.RemoveAll<DbContextOptions>();
            servicios.RemoveAll<IncapacidadesContext>();

            servicios.AddDbContext<IncapacidadesContext>(opciones => opciones.UseSqlite(_conexion));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        IHost anfitrion = base.CreateHost(builder);

        using IServiceScope alcance = anfitrion.Services.CreateScope();
        alcance.ServiceProvider.GetRequiredService<IncapacidadesContext>().Database.EnsureCreated();

        return anfitrion;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            _conexion.Dispose();
    }
}
