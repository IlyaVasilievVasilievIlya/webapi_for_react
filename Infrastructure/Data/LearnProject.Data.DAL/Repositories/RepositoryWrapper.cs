using LearnProject.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.Data.DAL.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        RepositoryAppDbContext Context { get; }

        public ICarModelRepository CarModelRepository { get; }
        public IUserRepository UserRepository { get;}
        public ICarRepository CarRepository { get; }


        public RepositoryWrapper(RepositoryAppDbContext context)
        {
            Context = context;
            CarModelRepository = new CarModelRepository(context);
            UserRepository = new UserRepository(context);
            CarRepository = new CarRepository(context);
        }

        public Task SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
