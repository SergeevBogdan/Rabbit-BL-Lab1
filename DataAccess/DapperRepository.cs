
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Rabbit.Entities;

namespace Rabbit.DataAccess
{
    public class DapperRepository : IRepository<RabbitModel>  // ← RabbitModel вместо Rabbit
    {
        private readonly string _connectionString;

        public DapperRepository()
        {
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

        public void Add(RabbitModel entity)  // ← RabbitModel
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute(
                    "INSERT INTO Rabbits (Id, Name, Breed, Age, Weight) VALUES (@Id, @Name, @Breed, @Age, @Weight)",
                    new { entity.Id, entity.Name, entity.Breed, entity.Age, entity.Weight });
            }
        }

        public void Delete(RabbitModel entity)  // ← RabbitModel
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM Rabbits WHERE Id = @Id", new { entity.Id });
            }
        }

        public IEnumerable<RabbitModel> ReadAll()  // ← RabbitModel
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<RabbitModel>("SELECT * FROM Rabbits").ToList();
            }
        }

        public RabbitModel ReadById(int id)  // ← RabbitModel
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<RabbitModel>("SELECT * FROM Rabbits WHERE Id = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public void Update(RabbitModel entity)  // ← RabbitModel
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute(
                    "UPDATE Rabbits SET Name = @Name, Breed = @Breed, Age = @Age, Weight = @Weight WHERE Id = @Id",
                    new { entity.Id, entity.Name, entity.Breed, entity.Age, entity.Weight });
            }
        }
    }
}
