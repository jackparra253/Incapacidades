using Api.Empleados;
using Api.Incapacidades;
using Api.Liquidacion;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder constructor = WebApplication.CreateBuilder(args);

        constructor.Services.AgregarBitakora(constructor.Configuration);

        WebApplication app = constructor.Build();

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseExceptionHandler();

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.MapearEmpleados();
        app.MapearIncapacidades();
        app.MapearLiquidacion();

        app.Run();
    }
}
