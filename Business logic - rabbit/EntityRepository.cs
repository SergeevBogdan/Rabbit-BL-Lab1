using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly RabbitDbContext _context;

        public EntityRepository(RabbitDbContext context)
        {
            _context = context;
            _context.Database.Log = sql => Console.WriteLine($"EF SQL: {sql}");

            // 🔥 СОЗДАЕМ ТАБЛИЦУ ЕСЛИ НЕ СУЩЕСТВУЕТ
            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            try
            {
                // Проверяем существование таблицы
                var tableExists = _context.Database.SqlQuery<int?>(
                    "SELECT OBJECT_ID('Rabbits')").FirstOrDefault();

                if (tableExists == null)
                {
                    Console.WriteLine("🔄 EF: Создаем таблицу Rabbits...");
                    _context.Database.ExecuteSqlCommand(@"
                CREATE TABLE Rabbits (
                    Id INT PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Breed NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    Weight INT NOT NULL
            )");
                    Console.WriteLine("✅ EF: Таблица Rabbits создана");
                }
                else
                {
                    Console.WriteLine("✅ EF: Таблица Rabbits существует");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EF: Ошибка создания таблицы: {ex.Message}");
                throw;
            }
        }

        public void Add(T entity)
        {
            try
            {
                Console.WriteLine($"🔍 EF: Добавляем объект типа {typeof(T).Name}");
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                Console.WriteLine($"✅ EF: Объект добавлен успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EF Add ошибка: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }

        // ... остальные методы без изменений
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> ReadAll()
        {
            return _context.Set<T>().ToList();
        }

        public T ReadById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            var existing = _context.Set<T>().Find(((Rabbit)(object)entity).Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }
    }
}