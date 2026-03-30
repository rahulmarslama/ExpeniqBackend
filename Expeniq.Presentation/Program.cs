using Expendiq.Domain.Entities.Users;
using Expendiq.Infrastructure.Data;
using Expeniq.Presentation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();