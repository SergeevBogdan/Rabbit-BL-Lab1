using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Business_logic___rabbit;

    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly MainDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository()
        {
            _context = new MainDbContext();
            _dbSet = _context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        public T ReadById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }

}
