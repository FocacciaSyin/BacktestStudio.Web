using BacktestStudio.Web.Components;
using BacktestStudio.Web.Services;
using ApexCharts;
using BacktestStudio.Repository;
using BacktestStudio.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register our services
builder.Services.AddSingleton<AppState>();
builder.Services.AddScoped<IApiService, MockApiService>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddHttpClient();

// Register ApexCharts
builder.Services.AddApexCharts();

builder.Services.AddDbContext<BackestStudioContext>(options =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
    options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
