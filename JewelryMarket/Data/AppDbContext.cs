using JewelryMarket.Entities;
using Microsoft.EntityFrameworkCore;

namespace JewelryMarket.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<JewelryItem> JewelryItems { get; set; }
    }
}
