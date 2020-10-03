using Dapper;
using DapperGenericRepositoryUnitOfWork.Model;
using DapperGenericRepositoryUnitOfWork.SeedWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperGenericRepositoryUnitOfWork.Repositories
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        public BreedRepository(IDbTransaction transaction) : base("Breed", transaction)
        {
        }

        public async Task<Breed> FindByNameAsync(string name)
        {
            var breeds = await Connection.QueryAsync<Breed>(
                "SELECT * FROM Breed WHERE Name = @Name",
                param: new { Name = name },
                transaction: Transaction
            );
            return breeds.FirstOrDefault();
        }
    }
}
