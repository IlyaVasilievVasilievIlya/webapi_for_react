using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnProject.Data.MigrationService
{
    /// <summary>
    /// сервис миграций
    /// </summary>
    public class MigrationService : IMigrationService
    {
        readonly ILogger<MigrationService> logger;

        public MigrationService(ILogger<MigrationService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// запуск миграции
        /// </summary>
        public void Migrate(DbContext context)
        {
            var contextName = context.GetType().Name;
            logger.LogInformation("Migrations have started for {ContextName}", contextName);
            context.Database.Migrate();
            logger.LogInformation("Migrations have finished for {contextName}", contextName);
        }
    }
}
