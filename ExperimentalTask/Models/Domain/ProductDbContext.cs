using Microsoft.EntityFrameworkCore;
using System;

namespace ExperimentalTask.Models.Domain
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Product { get; set; }
    }
}
