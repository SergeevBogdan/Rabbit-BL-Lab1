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
        public RabbitDbContext() : base(GetConnectionString())
        {
            Database.SetInitializer<RabbitDbContext>(null);
        }

        public DbSet<Rabbit> Rabbits { get; set; } 

        private static string GetConnectionString()
        {
            string basePath = Directory.GetCurrentDirectory();
            string dbPath = Path.Combine(basePath, "Database1.mdf");
            return $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // МИНИМАЛЬНАЯ КОНФИГУРАЦИЯ - только таблица и ключ
            modelBuilder.Entity<Rabbit>().ToTable("Rabbits");
            modelBuilder.Entity<Rabbit>().HasKey(r => r.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
