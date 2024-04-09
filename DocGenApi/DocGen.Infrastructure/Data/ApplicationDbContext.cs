using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocGen.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
    }
}