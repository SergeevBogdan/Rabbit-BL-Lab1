using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public class RabbitDbContext : DbContext
    {
        public RabbitDbContext() : base("RabbitDbConnection")
        {
            // Включаем ленивую загрузку и создание базы если не существует
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new CreateDatabaseIfNotExists<RabbitDbContext>());
        }

        public DbSet<Rabbit> Rabbits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Минимальная конфигурация - EF сам создаст таблицу по атрибутам
            modelBuilder.Entity<Rabbit>().ToTable("Rabbits");
            base.OnModelCreating(modelBuilder);
        }
    }
}
