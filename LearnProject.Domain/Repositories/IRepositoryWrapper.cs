using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.Domain.Repositories
{
    public interface IRepositoryWrapper
    {
        ICarModelRepository CarModelRepository { get; }
        ICarRepository CarRepository { get; }

        IUserRepository UserRepository { get; }

        Task SaveAsync();
    }
}
