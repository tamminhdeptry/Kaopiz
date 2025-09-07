using Kaopiz.Auth.Domain;
using Kaopiz.Auth.Application;
using Microsoft.EntityFrameworkCore;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Infrastructure
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserEntityConfig());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<UserEntity> Users { get; set; } = null!;

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTimeOffset.UtcNow;
            var currentUserId = RuntimeContext.CurrentUserId == Guid.Empty ? SystemConstant.SYSTEM_ID : RuntimeContext.CurrentUserId;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = now;
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.Status = CMasterData.Active;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Modified = now;
                    entry.Entity.ModifiedBy = currentUserId;
                }
            }
        }
    }
}
