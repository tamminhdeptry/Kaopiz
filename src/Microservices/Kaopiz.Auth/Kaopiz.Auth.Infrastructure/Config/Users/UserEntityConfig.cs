using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kaopiz.Auth.Infrastructure
{
    public class UserEntityConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.DisplayName)
                .HasMaxLength(200);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Type)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.Property(u => u.Created)
                .IsRequired();

            builder.Property(u => u.CreatedBy)
                .IsRequired();

            builder.HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}