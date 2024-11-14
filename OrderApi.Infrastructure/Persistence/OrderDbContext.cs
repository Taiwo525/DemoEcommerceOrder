using Microsoft.EntityFrameworkCore;
using OrderApi.Core.Entities;

namespace OrderApi.Infrastructure.Persistence
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }
}
