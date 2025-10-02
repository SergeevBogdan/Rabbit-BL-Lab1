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
                // Способ 1: Через ConfigurationManager с проверкой
                var connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings["RabbitDbConnection"];

                if (connectionStringSettings == null)
                {
                    Console.WriteLine("❌ Connection string не найден в App.config, используем fallback...");
                    // Способ 2: Fallback - создаем строку вручную
                    string basePath = Directory.GetCurrentDirectory();
                    string dbPath = Path.Combine(basePath, "Database1.mdf");
                    _connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
                }
                else
                {
                    _connectionString = connectionStringSettings.ConnectionString;
                    Console.WriteLine("✅ Connection string загружен из App.config");
                }

                Console.WriteLine($"📁 Путь к базе: {_connectionString}");
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

        // Остальные методы без изменений...
        public void Add(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Rabbits (Id, Name, Breed, Age, Weight) VALUES (@Id, @Name, @Breed, @Age, @Weight)";
                db.Execute(sql, entity);
                Console.WriteLine($"✅ Добавлено через Dapper: {entity.Id}");
            }
        }

        public void Delete(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Rabbits WHERE Id = @Id";
                db.Execute(sql, new { entity.Id });
                Console.WriteLine($"✅ Удалено через Dapper: {entity.Id}");
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var result = db.Query<T>("SELECT * FROM Rabbits").ToList();
                Console.WriteLine($"✅ Прочитано через Dapper: {result.Count} записей");
                return result;
            }
        }

        public T ReadById(int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var result = db.Query<T>("SELECT * FROM Rabbits WHERE Id = @Id", new { Id = id }).FirstOrDefault();
                Console.WriteLine($"✅ Найден через Dapper ID {id}: {result != null}");
                return result;
            }
        }

        public void Update(T entity)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Rabbits SET Name = @Name, Breed = @Breed, Age = @Age, Weight = @Weight WHERE Id = @Id";
                db.Execute(sql, entity);
                Console.WriteLine($"✅ Обновлено через Dapper: {entity.Id}");
            }
        }
    }
}
