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
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            // 🔥 СБРАСЫВАЕМ КЕШ ПОСЛЕ ИЗМЕНЕНИЙ
            _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                  .ForEach(e => e.State = EntityState.Detached);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

            // 🔥 СБРАСЫВАЕМ КЕШ ПОСЛЕ ИЗМЕНЕНИЙ
            _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                  .ForEach(e => e.State = EntityState.Detached);
        }

        public IEnumerable<T> ReadAll()
        {
            // 🔥 ОЧИЩАЕМ КЕШ ПЕРЕД ЧТЕНИЕМ
            _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                  .ForEach(e => e.State = EntityState.Detached);

            return _context.Set<T>().ToList();
        }

        public T ReadById(int id)
        {
            // 🔥 ОЧИЩАЕМ КЕШ ПЕРЕД ПОИСКОМ
            _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                  .ForEach(e => e.State = EntityState.Detached);

            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            var existing = _context.Set<T>().Find(((Rabbit)(object)entity).Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();

                // 🔥 СБРАСЫВАЕМ КЕШ ПОСЛЕ ИЗМЕНЕНИЙ
                _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                      .ForEach(e => e.State = EntityState.Detached);
            }
        }
    }
}
