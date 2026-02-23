using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //ДОРАБОТАТЬ ПОЛНОСТЬЮ!!!!!!!!!!!!
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Business_logic___rabbit;
    using Dapper;

    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(T item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Здесь примерный SQL - нужно адаптировать под структуру таблицы и свойства T
                var sql = $"INSERT INTO {typeof(T).Name}s (...) VALUES (...)";
                connection.Execute(sql, item);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = $"DELETE FROM {typeof(T).Name}s WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = $"SELECT * FROM {typeof(T).Name}s";
                return connection.Query<T>(sql).ToList();
            }
        }

        public T ReadById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
                return connection.QuerySingleOrDefault<T>(sql, new { Id = id });
            }
        }

        public void Update(T item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Аналогично Add - здесь SQL для обновления
                var sql = $"UPDATE {typeof(T).Name}s SET ... WHERE Id = @Id";
                connection.Execute(sql, item);
            }
        }
    }
}
