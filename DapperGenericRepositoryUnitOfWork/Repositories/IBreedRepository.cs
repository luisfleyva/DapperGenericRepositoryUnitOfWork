using DapperGenericRepositoryUnitOfWork.Model;
using DapperGenericRepositoryUnitOfWork.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DapperGenericRepositoryUnitOfWork.Repositories
{
    public interface IBreedRepository : IGenericRepository<Breed>
    {
        Task<Breed> FindByNameAsync(string name);
    }
}
