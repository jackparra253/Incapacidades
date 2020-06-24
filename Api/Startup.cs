using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Datos;
using IDatos;
using IAplicacion;
using Aplicacion;
using Dominio;
using IDominio;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api
{
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

            //Dominio
            services.AddScoped<ICalculadoraReconocimientoEconomico, CalculadoraReconocimientoEconomico>();

            //Servicios
            services.AddScoped<IIncapacidadServicio, IncapacidadServicio>();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            services.AddDbContext<IncapacidadesContext>(options => options.UseSqlite(Configuration.GetConnectionString("IncapacidadesContext"), b => b.MigrationsAssembly("Api")));

            services.AddDbContext<IncapacidadesContext>();
            services.AddScoped<IServicioDatos>(provider => provider.GetService<IncapacidadesContext>());
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
}
