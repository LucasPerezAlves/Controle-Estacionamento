using System.Globalization;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using Estacionamento.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddDbContext<EstacionamentoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRegistroEstacionamentoRepositorio, RegistroEstacionamentoRepositorio>();
builder.Services.AddScoped<ITabelaPrecoRepositorio, TabelaPrecoRepositorio>();
builder.Services.AddScoped<RegistradorDeSaida>();
builder.Services.AddSingleton<SeletorDeTabelaPreco>();
builder.Services.AddSingleton<CalculadoraTarifa>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Estacionamento}/{action=Index}/{id?}");

app.Run();
