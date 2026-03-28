using Expendiq.Application.Dtos;
using Expendiq.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Serilog.Context;
using System.Text.Json;

namespace Expendiq.Application.Extensions
{
    public class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var routeValues = httpContext.GetRouteData()?.Values;
            string controllerName = routeValues?["controller"]?.ToString() ?? "Unknown";
            string actionName = routeValues?["action"]?.ToString() ?? "Unknown";

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                NoContentException => StatusCodes.Status200OK,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new DataResult
            {
                Success = false,
                Status = httpContext.Response.StatusCode,
                Message = exception.Message
            };

            if (exception is DbUpdateException dbException)
            {
                if (dbException.InnerException is NpgsqlException npgException)
                {
                    response.Message = npgException.SqlState switch
                    {
                        "23503" => HandleReferentialIntegrity(actionName), // Foreign Key
                        "23505" => "This record already exists. Please use a different value.", // Unique
                        "23502" => "Required field is missing.", // NOT NULL
                        "23514" => "Invalid data format.", // Check constraint
                        _ => "Database constraint violation occurred."
                    };

                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = httpContext.Response.StatusCode;
                }
            }

            if (httpContext.Response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                response.Message = "An error occurred with the system.";

                response.Message = "An error occurred with the system.";
                using (LogContext.PushProperty("UserName", httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "UnAuthenticated"))
                using (LogContext.PushProperty("ControllerName", controllerName))
                using (LogContext.PushProperty("ActionName", actionName))
                using (LogContext.PushProperty("Message", exception.Message))
                using (LogContext.PushProperty("InnerException", exception.InnerException))
                using (LogContext.PushProperty("StackTrace", exception.StackTrace))
                {
                    Log.Error(response.Message);
                }
            }

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static string HandleReferentialIntegrity(string actionName)
        {
            return actionName switch
            {
                "Delete" => "Cannot delete this record because it is referenced by other data. Please remove references first.",
                "Create" => "Cannot create this record because the referenced data does not exist.",
                "Update" => "Cannot update this record because the new values violate foreign key constraints.",
                _ => "This operation violates data relationships."
            };
        }
    }
}