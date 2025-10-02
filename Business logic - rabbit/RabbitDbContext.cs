using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public class RabbitDbContext : DbContext
    {
        public RabbitDbContext() : base("RabbitDbConnection")
        {
            Database.SetInitializer<RabbitDbContext>(null);
            this.Database.Log = sql => Console.WriteLine($"EF SQL: {sql}");
        }

        public DbSet<Rabbit> Rabbits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rabbit>().ToTable("Rabbits");

            // 🔥 ВАЖНО: Указываем что Id НЕ автоинкрементный 🔥
            modelBuilder.Entity<Rabbit>().HasKey(r => r.Id);
            modelBuilder.Entity<Rabbit>().Property(r => r.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }
    }
}