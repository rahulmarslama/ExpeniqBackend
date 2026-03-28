using Expendiq.Domain.Entities.Users;
using Expendiq.Infrastructure.Data;
using IdentityServer.Configurations;
using IdentityServer.IdentityExtensions.ClaimsConfiguration;
using IdentityServer.IdentityExtensions.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Expeniq")
    ?? throw new InvalidOperationException("Connection string 'Expeniq' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                            options =>
                            {
                                options.User.RequireUniqueEmail = true;
                            })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Account/Login";
    config.LogoutPath = "/Account/Logout";
});

IIdentityServerBuilder identityServerBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddAspNetIdentity<ApplicationUser>()
//.AddConfigurationStore(options =>
//{
//    options.ConfigureDbContext = b =>
//        b.UseNpgsql(connectionString,
//            sql => sql.MigrationsAssembly(migrationsAssembly));
//    options.DefaultSchema = "idnsrv";
//})
//.AddOperationalStore(options =>
//{
//    options.ConfigureDbContext = b =>
//        b.UseNpgsql(connectionString,
//            sql => sql.MigrationsAssembly(migrationsAssembly));
//    options.DefaultSchema = "idnsrv";
//});
//Use this during development process
.AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
            .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
            .AddInMemoryApiScopes(InMemoryConfiguration.GetApiScopes())
            .AddInMemoryClients(InMemoryConfiguration.GetClients());
//.AddDeveloperSigningCredential();
identityServerBuilder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Unspecified;
});

identityServerBuilder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Unspecified;
});
if (builder.Environment.IsDevelopment())
{
    identityServerBuilder.AddDeveloperSigningCredential();
}
else
{
    var filePath = Path.Combine(builder.Environment.ContentRootPath, "identityserverCert.pfx");
    var certificate = new X509Certificate2(filePath, builder.Configuration["CertificateSettings:SigningKeyPassword"]);
    identityServerBuilder.AddSigningCredential(certificate);
}



builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsFactory>();

builder.Services.AddCors();
builder.Services.AddControllersWithViews();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(options =>
    {
        options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
}

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");
//app.MigrateDatabase();


app.Run();