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

        /// <summary>
        /// получить все модели авто
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">лимит</param>
        /// <returns>коллекция моделей</returns>
        protected override async Task<IEnumerable<CarModel>> ReadAllImplementationAsync(int offset, int limit)
        {
            var models = context
                .CarBrandModels
                .AsNoTracking();

            return await models.ToListAsync();
        }

        /// <summary>
        /// получить модель авто по id
        /// </summary>
        /// <param name="id">id модели машины</param>
        /// <returns>сущность CarModel (либо null)</returns>
        protected override async Task<CarModel?> ReadImplementationAsync(int id)
        {
            return await context.CarBrandModels.FindAsync(id);
        }

        /// <summary>
        /// создание сущности модели авто (добавить)
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override CarModel CreateImplementation(CarModel model) {
            context.CarBrandModels.Add(model);
            return model;
        }

        /// <summary>
        /// изменение сущности модели авто (добавить)
        /// </summary>
        /// <param name="model">сущность</param>
        protected override void UpdateImplementation(CarModel model) => context.CarBrandModels.Update(model);

        /// <summary>
        /// удаление сущности модели авто (добавить)
        /// </summary>
        /// <param name="model">сущность</param>
        protected override void DeleteImplementation(CarModel model) => context.CarBrandModels.Remove(model);

        /// <summary>
        /// запрос по умолчанию
        /// </summary>
        /// <returns>результат запроса</returns>
        protected override IQueryable<CarModel> QueryImplementation()
        {
            return context.CarBrandModels.AsNoTracking();
        }

    }
}
