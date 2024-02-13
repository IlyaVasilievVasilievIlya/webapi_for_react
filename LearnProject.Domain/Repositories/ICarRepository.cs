using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;

namespace LearnProject.Domain.Repositories
{
    /// <summary>
    /// репозиторий модели авто
    /// </summary>
    public interface ICarRepository : IRepository<Car, int>
    {
        PagedList<Car> GetCars(CarQueryParameters parameters);
    }
}
