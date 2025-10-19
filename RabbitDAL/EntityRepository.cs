using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RabbitDAL.RabbitDAL;
using RabbitModel;

namespace RabbitDAL
{
    public class EntityRepository : IRepository
    {
        private readonly RabbitDbContext _context;

        public EntityRepository(RabbitDbContext context)
        {
            _context = context;
        }

        public void Add(Rabbit rabbit)
        {
            _context.Rabbits.Add(rabbit);
            _context.SaveChanges();
        }

        public void Delete(Rabbit rabbit)
        {
            // ИСПРАВЛЕНИЕ: сначала находим объект в контексте
            var existing = _context.Rabbits.Find(rabbit.Id);
            if (existing != null)
            {
                _context.Rabbits.Remove(existing);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Rabbit> ReadAll()
        {
            return _context.Rabbits.ToList();
        }

        public Rabbit ReadById(int id)
        {
            return _context.Rabbits.Find(id);
        }

        public void Update(Rabbit rabbit)
        {
            var existing = _context.Rabbits.Find(rabbit.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(rabbit);
                _context.SaveChanges();
            }
        }
    }
}