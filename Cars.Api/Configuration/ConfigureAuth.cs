﻿using Google.Apis.Auth;
using LearnProject.Shared.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Cars.Api.Configuration
{
    internal static class ConfigureAuth
    {
        /// <summary>
        /// метод расширения для настройки аутентификации
        /// </summary>
        /// <param name="services">коллекция сервисов</param>
        /// <param name="configuration">объект конфигурации</param>
        /// <returns>коллекция сервисов</returns>
        internal static IServiceCollection AddAppAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            var settings = services.AddAppSettings<JwtSettings>(configuration);

            var tokenValidationParameters = new TokenValidationParameters
            {

                ValidIssuer = settings.Issuer, 
                ValidAudience = settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            return services;
        }
    }
}
