using TConsultigSA;
using Microsoft.Data.SqlClient;
using Dapper;
using BCrypt.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using TConsultigSA.Repositories;
using TConsultingSA.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios y dependencias
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<EmpleadoRepositorio>();
builder.Services.AddScoped<PuestoRepositorio>();
builder.Services.AddScoped<DepartamentoRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<IPermisoRepositorio, PermisoRepositorio>();
builder.Services.AddScoped<IHorasTrabajoRepositorio, HorasTrabajoRepositorio>();

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configuración de la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}"); // Cambiar la ruta predeterminada a Welcome

app.Run();
