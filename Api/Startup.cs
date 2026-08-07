using Bitakora.Empleados;
using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Bitakora.Persistencia;
using Bitakora.Salarios;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddScoped<IConsultarEmpleados, ConsultarEmpleados>();
        services.AddScoped<ICalcularFechas, CalcularFechas>();
        services.AddScoped<ICreadorIncapacidad, CreadorIncapacidad>();

        services.AddScoped<IIncapacidadServicio, IncapacidadServicio>();
        services.AddScoped<IEmpleadoServicio, EmpleadoServicio>();
        services.AddScoped<IResponsablePagoServicio, ResponsablePagoServicio>();
        services.AddScoped<ISalarioMinimoServicio, SalarioMinimoServicio>();
        services.AddProblemDetails();
        services.AddExceptionHandler<ExcepcionesDelDominioComoHttp>();

        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
        services.AddDbContext<IncapacidadesContext>(options => options.UseSqlite(Configuration.GetConnectionString("IncapacidadesContext"), b => b.MigrationsAssembly("Api")));

        services.AddDbContext<IncapacidadesContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandler();

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
