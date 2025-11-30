using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using RabbitDAL;
using RabbitModel;

namespace RabbitDAL
{
    /// <summary>
    /// Реализация репозитория с использованием Dapper
    /// </summary>
    public class DapperRepository : IRepository
    {
        private readonly string _connectionString;

        public DapperRepository()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\AceR\Desktop\Rabbit-Lab - 4\RabbitDAL\Database1.mdf;Integrated Security=True";
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

        public void Add(Rabbit rabbit)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute(
                    "INSERT INTO Rabbits (Id, Name, Breed, Age, Weight) VALUES (@Id, @Name, @Breed, @Age, @Weight)",
                    new { rabbit.Id, rabbit.Name, rabbit.Breed, rabbit.Age, rabbit.Weight });
            }
        }

        public void Delete(Rabbit rabbit)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM Rabbits WHERE Id = @Id", new { rabbit.Id });
            }
        }

        public IEnumerable<Rabbit> ReadAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<Rabbit>("SELECT * FROM Rabbits").ToList();
            }
        }

        public Rabbit ReadById(int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<Rabbit>("SELECT * FROM Rabbits WHERE Id = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public void Update(Rabbit rabbit)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute(
                    "UPDATE Rabbits SET Name = @Name, Breed = @Breed, Age = @Age, Weight = @Weight WHERE Id = @Id",
                    new { rabbit.Id, rabbit.Name, rabbit.Breed, rabbit.Age, rabbit.Weight });
            }
        }
    }
}