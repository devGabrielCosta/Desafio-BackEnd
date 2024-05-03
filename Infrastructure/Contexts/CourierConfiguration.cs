using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts
{
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Name")
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("Cnpj")
                .HasColumnType("char(14)")
                .IsRequired();

            builder
                .Property<DateTime>("BirthDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property("Cnh")
                .HasColumnType("char(12)")
                .IsRequired();

            builder
                .Property("CnhType")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("CnhImage")
                .HasColumnType("varchar");


            builder.HasKey("Id");
            builder.HasIndex("Cnpj").IsUnique();
            builder.HasIndex("Cnh").IsUnique();

            builder
                .HasMany(e => e.Notifications)
                .WithMany(e => e.Notifieds)
                .UsingEntity("Notifications",
                    j =>{   
                            j.Property("NotificationsId").HasColumnName("OrderId");
                            j.Property("NotifiedsId").HasColumnName("CourierId");
                    }
                );
        }
    }
}
