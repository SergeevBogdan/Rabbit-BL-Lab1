using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient; 
using System.IO;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Business_logic___rabbit
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository()
        {
            try
            {
                string basePath = Directory.GetCurrentDirectory();
                string dbPath = Path.Combine(basePath, "Database1.mdf");
                _connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";

                Console.WriteLine($"📁 База данных: {dbPath}");
                EnsureTableExists();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка инициализации Dapper: {ex.Message}");
                throw;
            }
        }

        private void EnsureTableExists()
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var tableExists = db.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Rabbits'");

                    if (tableExists == 0)
                    {
                        Console.WriteLine("🔄 Создаем таблицу Rabbits...");
                        db.Execute(@"
                        CREATE TABLE Rabbits (
                            Id INT PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL,
                            Breed NVARCHAR(100) NOT NULL,
                            Age INT NOT NULL,
                            Weight INT NOT NULL
                        )");
                        Console.WriteLine("✅ Таблица Rabbits создана");
                    }
                    else
                    {
                        Console.WriteLine("✅ Таблица Rabbits существует");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка создания таблицы: {ex.Message}");
                throw;
            }
        }

        public void Add(T entity)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var rabbit = entity as Rabbit;
                    if (rabbit == null) throw new ArgumentException("Entity must be of type Rabbit");

                    var sql = @"INSERT INTO Rabbits (Id, Name, Breed, Age, Weight) 
                           VALUES (@Id, @Name, @Breed, @Age, @Weight)";

                    var parameters = new
                    {
                        Id = rabbit.Id,
                        Name = rabbit.Name,
                        Breed = rabbit.Breed,
                        Age = rabbit.Age,
                        Weight = rabbit.Weight
                    };

                    int rowsAffected = db.Execute(sql, parameters);
                    Console.WriteLine($"✅ Добавлено через Dapper: {rabbit.Id}, строк затронуто: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка Dapper Add: {ex.Message}");
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var rabbit = entity as Rabbit;
                    if (rabbit == null) throw new ArgumentException("Entity must be of type Rabbit");

                    var sql = "DELETE FROM Rabbits WHERE Id = @Id";
                    var parameters = new { Id = rabbit.Id };

                    int rowsAffected = db.Execute(sql, parameters);
                    Console.WriteLine($"✅ Удалено через Dapper: {rabbit.Id}, строк затронуто: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка Dapper Delete: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<T> ReadAll()
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var result = db.Query<Rabbit>("SELECT * FROM Rabbits").ToList() as IEnumerable<T>;
                    Console.WriteLine($"✅ Прочитано через Dapper: {result.Count()} записей");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка Dapper ReadAll: {ex.Message}");
                return new List<T>();
            }
        }

        public T ReadById(int id)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var sql = "SELECT * FROM Rabbits WHERE Id = @Id"; 
                    var parameters = new { Id = id };

                    var result = db.Query<Rabbit>(sql, parameters).FirstOrDefault() as T;
                    Console.WriteLine($"✅ Найден через Dapper ID {id}: {result != null}");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка Dapper ReadById: {ex.Message}");
                return null;
            }
        }

        public void Update(T entity)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var rabbit = entity as Rabbit;
                    if (rabbit == null) throw new ArgumentException("Entity must be of type Rabbit");

                    var sql = @"UPDATE Rabbits 
                           SET Name = @Name, Breed = @Breed, Age = @Age, Weight = @Weight 
                           WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = rabbit.Id,
                        Name = rabbit.Name,
                        Breed = rabbit.Breed,
                        Age = rabbit.Age,
                        Weight = rabbit.Weight
                    };

                    int rowsAffected = db.Execute(sql, parameters);
                    Console.WriteLine($"✅ Обновлено через Dapper: {rabbit.Id}, строк затронуто: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка Dapper Update: {ex.Message}");
                throw;
            }
        }
    }
}
