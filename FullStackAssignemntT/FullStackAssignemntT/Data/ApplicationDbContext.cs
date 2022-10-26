using FullStackAssignemntT.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssignemntT.Data
{
    // 26.10 exted to use as Identity 
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Category> ShopCategories { get; set; }
        public DbSet<Product> ShopProducts { get; set; }
        public DbSet<Size> ShopSize { get; set; }
        public DbSet<ApplicationUser> ShopApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShopShoppingCart { get; set; }
    }
}
