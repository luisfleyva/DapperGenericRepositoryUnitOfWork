using DapperGenericRepositoryUnitOfWork.Repositories;
using DapperGenericRepositoryUnitOfWork.SeedWork;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperGenericRepositoryUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IBreedRepository _breedRepository;
        private ICatRepository _catRepository;

        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IBreedRepository BreedRepository
        {
            get { return _breedRepository ?? (_breedRepository = new BreedRepository(_transaction)); }
        }

        public ICatRepository CatRepository
        {
            get { return _catRepository ?? (_catRepository = new CatRepository(_transaction)); }
        }

        private void resetRepositories()
        {
            _breedRepository = null;
            _catRepository = null;
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }



        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
