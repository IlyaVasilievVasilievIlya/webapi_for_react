using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
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
            var settings = new JwtSettings();
            var section = configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(section);
            section.Bind(settings);

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
                })
            .AddJwtBearer("ExternalJwtScheme", options =>
            {

                options.TokenHandlers.Clear();
                options.TokenHandlers.Add(new GoogleTokenValidator());

            });

            return services;
        }
    }


}
