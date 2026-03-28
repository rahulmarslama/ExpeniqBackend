using Expendiq.Application.Dtos;
using Expendiq.Application.Extensions;
using Expendiq.Application.IServices;
using Expendiq.Application.Services;
using Expendiq.Domain.Entities.Users;
using Expendiq.Infrastructure.Data;
using Expendiq.Infrastructure.IRepositories;
using Expendiq.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Expeniq.Presentation
{
    public static class RegisterStartupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Expeniq")
                ?? throw new InvalidOperationException("Connection string 'Expeniq' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(connectionString));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new CustomValidationResponseActionFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi();
            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "v1";
                config.Title = "Expendiq API";
                config.Version = "v1";
                config.Description = "Transaction Management System API";

                config.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme"
                });

                config.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor());
            });

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();




            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };
                // base-address of your IdentityServer
                config.Authority = builder.Configuration["IdentityServerConfigurations:AuthorityUrl"];

                // name of the API resource
                config.Audience = builder.Configuration["IdentityServerConfigurations:Audiance"];

                config.MapInboundClaims = false;

                config.RequireHttpsMetadata = false;
            });
            builder.Services.AddCors();

            ConfigureLogger(builder.Configuration);
            builder.Services.AddTransient<ExceptionHandler>();
            return builder;
        }

        /// <summary>
        /// This static method is used to configure the Serilog Logger.
        /// As the logging is done only in the global Exception Handling and no DI injections are required ,IConfiguration is passed in this function.
        /// If any logging is required in any other services, make sure to pass the above WebApplicationBuilder as an argument.
        /// And by doing so, adding DI injection can be done easily by using the methods provided by the dependency injection container to register and configure services. 
        /// </summary>
        /// <remarks>Author: Rahul Mars Lama</remarks>
        /// <param name="configuration"></param>
        private static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }

    public static class RegisterStartupMiddlewares
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            app.MapOpenApi();
            app.UseOpenApi();
            app.UseSwaggerUi();

            app.UseMiddleware<ExceptionHandler>();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.ContentType = "application/json";
                    DataResult response;
                    response = new DataResult
                    {
                        Success = false,
                        Status = context.Response.StatusCode,
                        Message = "UnAuthorized"
                    };
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            //app.MigrateDatabase();

            // Migrate database on startup
            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    db.Database.Migrate();
            //}
            return app;
        }
    }
}

