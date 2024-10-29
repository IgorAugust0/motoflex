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
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property<DateTime>("CreatedAt")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            builder
                .Property("DeliveryFee")
                .HasPrecision(5, 2)
                .IsRequired();

            builder
                .Property("Status")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property<Guid?>("RenterId")
                .IsRequired(false);

            builder.HasKey("Id");

            // Configures one-to-many relationship between Renter and Orders
            builder
                .HasOne(o => o.Renter)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.RenterId);
        }
    }
}
