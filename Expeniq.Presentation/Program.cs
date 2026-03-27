using Expendiq.Application.IServices;
using Expendiq.Application.Services;
using Expendiq.Domain.Entities;
using Expendiq.Infrastructure.Data;
using Expendiq.Infrastructure.IRepositories;
using Expendiq.Infrastructure.Repositories;
using Expeniq.Presentation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
        //.AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/api/auth/login";
//        options.LogoutPath = "/api/auth/logout";
//        options.ExpireTimeSpan = TimeSpan.FromDays(7);
//        options.SlidingExpiration = true;
//        options.Cookie.HttpOnly = true;
//        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
//        options.Cookie.SameSite = SameSiteMode.Lax;
//    });

//builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    try
    {
        // Apply pending migrations
        await db.Database.MigrateAsync();
        Console.WriteLine("? Database migrations applied successfully");

        // Seed initial data
        await DataSeeder.SeedAsync(userManager, db);
        Console.WriteLine("? Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Error during migration/seeding: {ex.Message}");
        throw;
    }
}

app.MapOpenApi();
app.UseOpenApi();
app.UseSwaggerUi();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowReactApp");
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();

//// Migrate database on startup
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    db.Database.Migrate();
//}

app.Run();
