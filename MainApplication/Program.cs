using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Data;
using RottenPotatoes.Services;
using System.Reflection;
using RottenPotatoes.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PotatoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PotatoContext") ?? throw new InvalidOperationException("Connection string 'PotatoContext' not found.")));

builder.Services.AddAuthentication(CustomTokenAuthOptions.DefaultScheme)
    .AddScheme<CustomTokenAuthOptions, CustomTokenAuthHandler>(CustomTokenAuthOptions.DefaultScheme, options => { });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor(); // Required for session manager
builder.Services.AddScoped<SessionManager>(); // Register your class

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession(); // Enable session

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
Console.WriteLine("Resources: " + string.Join(", ", resourceNames)); // View in output

app.Run();
