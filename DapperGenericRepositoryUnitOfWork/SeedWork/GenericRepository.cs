using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Linq;
using System.ComponentModel;

namespace DapperGenericRepositoryUnitOfWork.SeedWork
{
    public abstract class GenericRepository<T> : RepositoryBase, IGenericRepository<T> where T : class
    {
        private readonly string _tableName;

        protected GenericRepository(string tableName, IDbTransaction transaction)
            : base(transaction)
        {
            _tableName = tableName;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>(
                $"SELECT * FROM {_tableName}", transaction: Transaction);
        }

        public async Task DeleteAsync(int id)
        {
            await Connection.ExecuteAsync(
                $"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id }, transaction: Transaction);
        }

        public async Task<T> GetAsync(int id)
        {
            var result = await Connection.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id }, transaction: Transaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

            return result;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();
            inserted += await Connection.ExecuteAsync(query, list);
            return inserted;
        }

        public async Task<int> AddAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            var newId = await Connection.ExecuteScalarAsync<int>(
                insertQuery, t, transaction: Transaction);
            return newId;
        }

        public async Task ReplaceAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();
            await Connection.ExecuteAsync(updateQuery, t, transaction: Transaction);
        }



        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            insertQuery.Append("; SELECT SCOPE_IDENTITY()");

            return insertQuery.ToString();
        }
    }
}
