using TConsultigSA;
using Microsoft.Data.SqlClient;
using Dapper;
using BCrypt.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using TConsultigSA.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<EmpleadoRepositorio>();

// Registrar los repositorios
builder.Services.AddScoped<PuestoRepositorio>();
builder.Services.AddScoped<DepartamentoRepositorio>();
//otra parte trabajada 
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<IPermisoRepositorio, PermisoRepositorio>();



var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
