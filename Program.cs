using LojaVirtual.Data;
using LojaVirtual.Models;
using LojaVirtual.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// contexto
builder.Services
    .AddDbContext<AppDbContext>(db => db
        .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LojaVirtual"));

// identity
builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// dependency injection
builder.Services.AddScoped<MercadoPagoService, MercadoPagoService>();
builder.Services.AddScoped<pedidoService, pedidoService>();
builder.Services.AddScoped<RegisterService, RegisterService>();
builder.Services.AddScoped<LoginService, LoginService>();
builder.Services.AddScoped<CarrinhoService, CarrinhoService>();
builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<MailService, MailService>();

builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();