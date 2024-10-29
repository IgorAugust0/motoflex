using Motoflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Motoflex.Infrastructure.Contexts
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.Property<Guid>("Id").IsRequired();
            builder.HasKey("Id");
        }
    }
}
