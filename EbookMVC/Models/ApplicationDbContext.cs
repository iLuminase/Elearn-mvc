using Microsoft.EntityFrameworkCore;

namespace EbookMVC.Models;


    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
