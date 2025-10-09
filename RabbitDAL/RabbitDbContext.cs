using System.Data.Entity;
using RabbitModel;

namespace RabbitDAL
{
    public class RabbitDbContext : DbContext
    {
        public RabbitDbContext() : base(GetConnectionString())
        {
            Database.SetInitializer<RabbitDbContext>(null);
        }

        private static string GetConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\AceR\Desktop\Rabbit-Lab - 4\RabbitDAL\Database1.mdf;Integrated Security=True";
        }

        public DbSet<Rabbit> Rabbits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Rabbit>().ToTable("Rabbits");
            modelBuilder.Entity<Rabbit>().HasKey(r => r.Id);
            modelBuilder.Entity<Rabbit>().Property(r => r.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
    }
}