using DapperGenericRepositoryUnitOfWork.Model;
using DapperGenericRepositoryUnitOfWork.SeedWork;
using System.Data;

namespace DapperGenericRepositoryUnitOfWork.Repositories
{
    public class CatRepository : GenericRepository<Cat>, ICatRepository
    {
        public CatRepository(IDbTransaction transaction) : base("Cat", transaction)
        {
        }
    }
}
