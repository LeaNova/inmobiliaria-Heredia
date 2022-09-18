using System.Security.Claims;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Usuario/Login";
            options.LogoutPath = "/Usuario/Logout";
            options.AccessDeniedPath = "/Home/Restringido";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

builder.Services.AddAuthorization(options => {
    //options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
});

builder.Services.AddTransient<IRepositorio<Propietario>, RepositorioPropietario>();
builder.Services.AddTransient<IRepositorioPropietario, RepositorioPropietario>();

builder.Services.AddTransient<IRepositorio<Inquilino>, RepositorioInquilino>();
builder.Services.AddTransient<IRepositorioInquilino, RepositorioInquilino>();

builder.Services.AddTransient<IRepositorio<Inmueble>, RepositorioInmueble>();
builder.Services.AddTransient<IRepositorioInmueble, RepositorioInmueble>();

builder.Services.AddTransient<IRepositorio<Contrato>, RepositorioContrato>();
builder.Services.AddTransient<IRepositorioContrato, RepositorioContrato>();

builder.Services.AddTransient<IRepositorio<Pago>, RepositorioPago>();
builder.Services.AddTransient<IRepositorioPago, RepositorioPago>();

builder.Services.AddTransient<IRepositorio<Usuario>, RepositorioUsuario>();
builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
/*
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
*/
