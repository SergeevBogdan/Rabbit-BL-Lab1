using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RabbitModel;

namespace RabbitDAL
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly RabbitDbContext _context;

        public EntityRepository(RabbitDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

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
            var existing = _context.Set<T>().Find(entity.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges(); 
            }
        }
    }
}