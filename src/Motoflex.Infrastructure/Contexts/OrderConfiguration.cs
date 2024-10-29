using Motoflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Motoflex.Infrastructure.Contexts
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .IsRequired();

            builder
                .Property(m => m.DeliveryFee)
                .HasPrecision(5, 2)
                .HasColumnName("delivery_fee")
                .IsRequired();

            builder
                .Property(m => m.Status)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasColumnName("status")
                .IsRequired();

            builder
                .Property(m => m.RenterId)
                .HasColumnName("renter_id")
                .IsRequired(false);

            builder.HasKey(m => m.Id);

            // Configures one-to-many relationship between Renter and Orders
            builder
                .HasOne(o => o.Renter)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.RenterId);
        }
    }
}
