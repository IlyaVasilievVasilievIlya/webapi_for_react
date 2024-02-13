using LearnProject.Domain.Repositories;
using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LearnProject.Domain.Models;
using LearnProject.Shared.Common;

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

        public override Task<Car?> FindByKeyAsync(int key)
        {
            return FindByCondition(entity => entity.CarId == key).AsTracking().Include(entity => entity.CarModel).FirstOrDefaultAsync();
        }

        public PagedList <Car> GetCars(CarQueryParameters parameters)
        {
            var cars = FindByCondition(car => 
                (car.Color ?? string.Empty).ToLower().Contains(parameters.Color.ToLower())
                && (car.CarModel!.Brand + " " + car.CarModel.Name).ToLower().Contains(parameters.CarName.ToLower())
                && car.CarModel!.Brand.ToLower().Contains(parameters.Brand.ToLower())
                && car.CarModel!.Name.ToLower().Contains(parameters.Model.ToLower()));

            return PagedList<Car>.ToPagedList(cars
                .OrderBy(car => car.CarId),
                parameters.PageNumber,
                parameters.PageSize);
        }

        public override IQueryable<Car> ReadAll()
        {
            return base.ReadAll().Include(car => car.CarModel);
        }
    }
}
