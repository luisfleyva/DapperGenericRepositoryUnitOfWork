using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DapperGenericRepositoryUnitOfWork.SeedWork
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<int> AddRangeAsync(IEnumerable<T> list);
        Task ReplaceAsync(T t);
        Task<int> AddAsync(T t);
    }
}
