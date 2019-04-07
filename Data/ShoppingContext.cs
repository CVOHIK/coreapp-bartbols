using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebApplicationShop.Models;

namespace WebApplicationShop.Data
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext()//: base("name=DefaultConnection")
        {
            Database.SetInitializer<ShoppingContext>(null);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderObject> OrderObjects { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<WebApplicationShop.Models.ShoppingCartViewModel> ShoppingCartViewModels { get; set; }
        //public DbSet<ShoppingObject> ShoppingObjects { get; set; }

    }
}