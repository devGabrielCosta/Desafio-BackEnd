using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Context
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property<DateTime>("Criado")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            builder
                .Property("Valor")
                .HasPrecision(5,2)
                .IsRequired();

            builder
                .Property("Situacao")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property<Guid?>("EntregadorId")
                .IsRequired(false);

            builder.HasKey("Id");
            builder.HasOne("Entregador").WithMany("Pedidos").HasForeignKey("EntregadorId");
        }
    }
}
