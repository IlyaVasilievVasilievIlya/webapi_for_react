using LearnProject.Data.DAL;
using LearnProject.Data.MigrationService;

namespace Cars.Api.Configuration
{
    internal static class DbInitializer
    {
        /// <summary>
        /// автомиграция БД
        /// </summary>
        /// <param name="app">класс приложения</param>
        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryAppDbContext>();

                migrationService.Migrate(context);
            }
        }
    }
}
