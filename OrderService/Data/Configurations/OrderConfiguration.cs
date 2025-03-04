using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id).HasName("PK__Orders__3214EC078FD166B6");

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique()
                .HasDatabaseName("UQ__Orders__CAC5E743242B9027");

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(o => o.CustomerName).HasMaxLength(100);
            builder.Property(o => o.OrderNumber).HasMaxLength(20);
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18, 2)");

            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__3D5E1FD2");
        }
    }
}