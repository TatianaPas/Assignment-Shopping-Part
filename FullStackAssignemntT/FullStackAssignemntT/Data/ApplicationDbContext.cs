using FullStackAssignemntT.Models;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssignemntT.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Category> ShopCategories { get; set; }
        public DbSet<Product> ShopProducts { get; set; }
    }
}
