using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data.MigrationService
{
    /// <summary>
    /// сервис миграций
    /// </summary>
    public interface IMigrationService
    {
        void Migrate(DbContext context);
    }
}
