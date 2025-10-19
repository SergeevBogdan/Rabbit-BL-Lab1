using System;
using System.Collections.Generic;
using RabbitModel;

namespace RabbitDAL
{
    using System.Collections.Generic;
    using RabbitModel;

    namespace RabbitDAL
    {
        public interface IReadRepository
        {
            Rabbit ReadById(int id);
            IEnumerable<Rabbit> ReadAll();
        }

        public interface IWriteRepository
        {
            void Add(Rabbit rabbit);
            void Update(Rabbit rabbit);
            void Delete(Rabbit rabbit);
        }

        public interface IRepository : IReadRepository, IWriteRepository
        {
        }
    }
} 