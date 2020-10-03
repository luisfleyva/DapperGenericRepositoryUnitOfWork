using System;
using System.Collections.Generic;
using System.Text;

namespace DapperGenericRepositoryUnitOfWork.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
