using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentityServer()
    //.AddConfigurationStore(options =>
    //{
    //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
    //        sql => sql.MigrationsAssembly(assembly));
    //})
    //.AddOperationalStore(options =>
    //{
    //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
    //        sql => sql.MigrationsAssembly(assembly));
    //})
    //.AddSigningCredential(certificate);
    .AddInMemoryClients(Configuration.Clients)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddTestUsers(Configuration.Users)
    .AddDeveloperSigningCredential();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
