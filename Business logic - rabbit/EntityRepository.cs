using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic; 
using System.Data.Entity;
using System.Linq;
namespace Business_logic___rabbit
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly RabbitDbContext _context;

        public EntityRepository(RabbitDbContext context)
        {
            _context = context;
            Console.WriteLine("✅ EntityRepository создан");
        }

        public void Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                Console.WriteLine($"✅ Добавлено через EF: {entity.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка EF Add: {ex.Message}");
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                Console.WriteLine($"✅ Удалено через EF: {entity.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка EF Delete: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<T> ReadAll()
        {
            try
            {
                var result = _context.Set<T>().ToList();
                Console.WriteLine($"✅ Прочитано через EF: {result.Count} записей");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка EF ReadAll: {ex.Message}");
                return new List<T>();
            }
        }

        public T ReadById(int id)
        {
            try
            {
                var result = _context.Set<T>().FirstOrDefault(e => e.Id == id);
                Console.WriteLine($"✅ Найден через EF ID {id}: {result != null}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка EF ReadById: {ex.Message}");
                return null;
            }
        }

        public void Update(T entity)
        {
            try
            {
                var existing = _context.Set<T>().Find(entity.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                    Console.WriteLine($"✅ Обновлено через EF: {entity.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка EF Update: {ex.Message}");
                throw;
            }
        }
    }
}
