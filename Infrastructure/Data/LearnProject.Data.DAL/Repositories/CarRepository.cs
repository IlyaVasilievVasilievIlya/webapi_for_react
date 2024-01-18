using LearnProject.Domain.Repositories;
using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data.DAL.Repositories
{
    /// <summary>
    /// реализация репозитория работы с авто
    /// </summary>
    public sealed class CarRepository : AbstractRepository<Car, int>, ICarRepository
    {
        /// <summary>
        /// контекст БД
        /// </summary>
        RepositoryAppDbContext context;

        public CarRepository(RepositoryAppDbContext context)
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
        /// получить список машин
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция сущностей Car</returns>
        protected override async Task<IEnumerable<Car>> ReadAllImplementationAsync(int offset, int limit)
        {
            var cars = context
                .Cars
                .Include(e => e.CarModel)
                .AsNoTracking();

            cars = cars
                .OrderBy(car => car.CarId)
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, limit));

            return await cars.ToListAsync();
        }

        /// <summary>
        /// получить авто по id
        /// </summary>
        /// <param name="id">id машины</param>
        /// <returns>сущность Car (либо null)</returns>
        protected override async Task<Car?> ReadImplementationAsync(int id) => await context.Cars.FindAsync(id);

        /// <summary>
        /// пометить авто на добавление
        /// </summary>
        /// <param name="car">сущность машины</param>
        protected override Car CreateImplementation(Car car)
        {
            context.Cars.Add(car);

            return car;
        }
        
        /// <summary>
        /// пометить авто на изменение
        /// </summary>
        /// <param name="car">сущность</param>
        protected override void UpdateImplementation(Car car) => context.Update(car);

        /// <summary>
        /// пометить авто на удаление
        /// </summary>
        /// <param name="car">сущность</param>
        protected override void DeleteImplementation(Car car) => context.Cars.Remove(car);

        /// <summary>
        /// запрос по умолчанию (получение всех сущностей)
        /// </summary>
        /// <returns>результат запроса</returns>
        protected override IQueryable<Car> QueryImplementation()
        {
            return context.Cars.AsNoTracking();
        }
    }
}
