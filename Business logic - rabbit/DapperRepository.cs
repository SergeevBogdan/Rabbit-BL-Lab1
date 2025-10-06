using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Business_logic___rabbit
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository()
        {
            //_connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\AceR\Desktop\Rabbit-Lab - 4\Business logic - rabbit\Database1.mdf;Integrated Security=True";
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\Rabbit-BL-Lab1\Business logic - rabbit\Database1.mdf;Integrated Security=True";
            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();
                var tableExists = db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Rabbits'");

                if (tableExists == 0)
                {
                    db.Execute(@"
                        CREATE TABLE Rabbits (
                            Id INT PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL,
                            Breed NVARCHAR(100) NOT NULL,
                            Age INT NOT NULL,
                            Weight INT NOT NULL
                        )");
                }
            }
        }

        public void Add(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var rabbit = entity as Rabbit;
                db.Execute(
                    "INSERT INTO Rabbits (Id, Name, Breed, Age, Weight) VALUES (@Id, @Name, @Breed, @Age, @Weight)",
                    new { rabbit.Id, rabbit.Name, rabbit.Breed, rabbit.Age, rabbit.Weight });
            }
        }

        public void Delete(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM Rabbits WHERE Id = @Id", new { entity.Id });
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<Rabbit>("SELECT * FROM Rabbits").ToList() as IEnumerable<T>;
            }
        }

        public T ReadById(int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<Rabbit>("SELECT * FROM Rabbits WHERE Id = @Id", new { Id = id }).FirstOrDefault() as T;
            }
        }

        public void Update(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var rabbit = entity as Rabbit;
                db.Execute(
                    "UPDATE Rabbits SET Name = @Name, Breed = @Breed, Age = @Age, Weight = @Weight WHERE Id = @Id",
                    new { rabbit.Id, rabbit.Name, rabbit.Breed, rabbit.Age, rabbit.Weight });
            }
        }
    }
}