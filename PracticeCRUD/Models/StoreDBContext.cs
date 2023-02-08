using Microsoft.EntityFrameworkCore;

namespace PracticeCRUD.Models
{
    public class StoreDBContext:DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> options):base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }

    }
}
