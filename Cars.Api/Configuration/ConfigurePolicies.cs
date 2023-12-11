using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;

namespace Cars.Api.Configuration
{
    internal static class ConfigurePolicies
    {
        /// <summary>
        /// метод расширения для добавления политик
        /// </summary>
        /// <param name="services">коллекция DI</param>
        /// <returns>коллекция DI</returns>
        internal static void AddAppPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(AppPolicies.ViewCars, policy => policy.RequireRole(AppRoles.Manager, AppRoles.Admin, AppRoles.SuperUser, AppRoles.User));
            options.AddPolicy(AppPolicies.EditCars, policy => policy.RequireRole(AppRoles.Manager, AppRoles.Admin, AppRoles.SuperUser));
            options.AddPolicy(AppPolicies.ViewUsers, policy => policy.RequireRole(AppRoles.Admin, AppRoles.SuperUser));
            options.AddPolicy(AppPolicies.EditUsers, policy => policy.RequireRole(AppRoles.Admin, AppRoles.SuperUser));
            options.AddPolicy(AppPolicies.EditRoles, policy => policy.RequireRole(AppRoles.SuperUser));
        }
    }
}
