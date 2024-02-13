using LearnProject.Domain.Entities;
using LearnProject.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data.DAL.Repositories
{
    /// <summary>
    /// реализация репозитория работы с моделями авто
    /// </summary>
    public class CarModelRepository : AbstractRepository<CarModel, int>, ICarModelRepository
    {
        /// <summary>
        /// контекст БД
        /// </summary>
        RepositoryAppDbContext context;

        public CarModelRepository(RepositoryAppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// получение контекста для AbstractRepository
        /// </summary>
        protected override RepositoryAppDbContext Context
        {
            get => context;
            set => context = value;
        }

        public override Task<CarModel?> FindByKeyAsync(int key)
        {
            return FindByCondition(entity => entity.CarModelId == key).AsTracking().FirstOrDefaultAsync();
        }
    }
}
