using Microsoft.EntityFrameworkCore;
using Datos;
using IDatos;
using IAplicacion;
using Aplicacion;

namespace Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        //Aplicacion
        services.AddScoped<IConsultarEmpleados, ConsultarEmpleados>();
        services.AddScoped<ICalcularFechas, CalcularFechas>();
        services.AddScoped<ICreadorIncapacidad, CreadorIncapacidad>();

        //Servicios
        services.AddScoped<IIncapacidadServicio, IncapacidadServicio>();
        services.AddScoped<IEmpleadoServicio, EmpleadoServicio>();
        services.AddScoped<IResponsablePagoServicio, ResponsablePagoServicio>();
        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
        services.AddDbContext<IncapacidadesContext>(options => options.UseSqlite(Configuration.GetConnectionString("IncapacidadesContext"), b => b.MigrationsAssembly("Api")));

        services.AddDbContext<IncapacidadesContext>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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