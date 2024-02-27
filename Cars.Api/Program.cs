using Cars.Api.Configuration;
using Cars.Api.Controllers.Cars.FIlters;
using Cars.Api.Middleware;
using LearnProject.Data.DAL;
using LearnProject.Domain.Entities;
using LearnProject.Shared.Common;
using LearnProject.Shared.Common.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddHttpContextAccessor();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithCorrelationId()
    .CreateLogger();
builder.Host.UseSerilog(logger, true);


services.AddDbContext<RepositoryAppDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));

services.AddAppMinio(builder.Configuration);

services.AddAppSettings<FileUploadSettings>(builder.Configuration);
services.AddAppSettings<ExternalProviders>(builder.Configuration);
services.AddAppSettings<EmailSettings>(builder.Configuration);

services.AddScoped<FileValidationActionFilter>();

services.AddAppIdentity();

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

app.EnsureMinioBucketExists();

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
