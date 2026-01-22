using Microsoft.EntityFrameworkCore;
using Noregold.Entities;
using Noregold.Entities.Interface;
using Noregold.Infrastructure;
using Noregold.Infrastructure.Interface;
using Noregold.Service.Interrface;
using Noregold.Service.Service;
using Noregold.Web.Constants;
using System;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContextPool<AppDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString(AppConfig.MyApp.DbConnetion));
});

builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IInventoryService, InventoryService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

await using var app = builder.Build();

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "catchall",
    pattern: "{*url}",
    defaults: new { controller = "Home", action = "Index" });


await app.RunAsync();
