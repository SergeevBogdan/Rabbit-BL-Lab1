using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace DataAccessLayer
{

    public class MainDbContext : DbContext
    {
        public MainDbContext() : base("name=DefaultConnection") { }

        public DbSet<Rabbit> Rabbits { get; set; }
    }
}
