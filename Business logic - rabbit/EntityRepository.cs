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
            Console.WriteLine("✅ EF Repository создан");
        }

        public void Add(T entity)
        {
            try
            {
                Console.WriteLine($"🔍 EF: Пытаемся добавить кролика ID {((Rabbit)(object)entity).Id}");
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                Console.WriteLine($"✅ EF: Успешно добавлен кролик ID {((Rabbit)(object)entity).Id}");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Ошибки валидации EF
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"❌ EF Validation: {validationError.PropertyName} - {validationError.ErrorMessage}");
                    }
                }
                throw;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                Console.WriteLine($"❌ SQL ошибка: {sqlEx.Message}");
                Console.WriteLine($"🔍 SQL Number: {sqlEx.Number}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EF Add ошибка: {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Inner2: {ex.InnerException.InnerException.GetType().Name}: {ex.InnerException.InnerException.Message}");
                    }
                }
                throw;
            }
        }

        // Остальные методы остаются простыми
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
