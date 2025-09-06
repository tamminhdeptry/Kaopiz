using Microsoft.EntityFrameworkCore;

namespace CET.Repository
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
