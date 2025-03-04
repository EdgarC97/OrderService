using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id).HasName("PK__Users__3214EC078AA8CB90");

            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("UQ__Users__536C85E4E33AFFCC");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("UQ__Users__A9D10534C0919EB9");

            builder.Property(u => u.Email).HasMaxLength(100);
            builder.Property(u => u.PasswordHash).HasMaxLength(255);
            builder.Property(u => u.Username).HasMaxLength(50);
        }
    }
}