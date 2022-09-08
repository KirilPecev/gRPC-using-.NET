using GrpcService.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace GrpcService.Persistence
{
    public class BlazorDbContext : DbContext
    {
        public BlazorDbContext(DbContextOptions<BlazorDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
