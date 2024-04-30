using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Context
{
    public class MotoConfiguration : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Ano")
                .IsRequired();

            builder
                .Property("Modelo")
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder
                .Property("Placa")
                .IsRequired()
                .HasColumnType("char(7)");


            builder.HasKey("Id");
            builder.HasIndex("Placa").IsUnique();
        }
    }
}
