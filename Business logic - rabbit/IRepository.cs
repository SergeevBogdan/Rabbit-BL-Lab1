using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);
        void Delete(T entity);
        IEnumerable<T> ReadAll();
        T ReadById(int id); 
        void Update(T entity);
    }
}
