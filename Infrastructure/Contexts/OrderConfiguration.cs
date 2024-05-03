using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts
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
                .HasPrecision(5,2)
                .IsRequired();

            builder
                .Property("Status")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property<Guid?>("CourierId")
                .IsRequired(false);

            builder.HasKey("Id");
            builder.HasOne(e => e.Courier).WithMany(e => e.Orders).HasForeignKey(e => e.CourierId);
        }
    }
}
