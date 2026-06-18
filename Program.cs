using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

var builder = WebApplication.CreateBuilder(args);

// session state - must be added before AddControllersWithViews (Chapter 9)
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

// EF Core dependency injection (Chapter 4)
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("TodoContext")));

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

// must be called before routes are mapped (Chapter 9)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=SignIn}/{id?}");

// create / update the database automatically on first run
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    context.Database.Migrate();
}

app.Run();
