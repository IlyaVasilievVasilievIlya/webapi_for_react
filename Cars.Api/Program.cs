using Cars.Api.Configuration;
using Cars.Api.Middleware;
using LearnProject.Data.DAL;
using LearnProject.Domain.Entities;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog(logger, true);


services.AddDbContext<RepositoryAppDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));


services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RepositoryAppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = false;
});

services.AddAppAuth(configuration);

services.AddAuthorization(options =>
{
    options.AddAppPolicies();

});

services.AddAppSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAppCors();

services.AddAppServices();

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



var app = builder.Build();

app.ApplyMigrations();

app.UseAppCors();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
