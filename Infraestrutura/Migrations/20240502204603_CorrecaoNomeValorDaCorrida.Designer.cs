﻿// <auto-generated />
using System;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infraestrutura.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240502204603_CorrecaoNomeValorDaCorrida")]
    partial class CorrecaoNomeValorDaCorrida
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dominio.Entities.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Dominio.Entities.Entregador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Cnh")
                        .IsRequired()
                        .HasColumnType("char(12)");

                    b.Property<string>("CnhImagem")
                        .HasColumnType("varchar");

                    b.Property<string>("CnhTipo")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("char(14)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("Date");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.HasIndex("Cnh")
                        .IsUnique();

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.ToTable("Entregador");
                });

            modelBuilder.Entity("Dominio.Entities.Locacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<Guid>("EntregadorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Inicio")
                        .HasColumnType("Date");

                    b.Property<Guid>("MotoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Plano")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<DateTime>("PrevisaoDevolucao")
                        .HasColumnType("Date");

                    b.Property<DateTime>("Termino")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.HasIndex("MotoId");

                    b.ToTable("Locacao");
                });

            modelBuilder.Entity("Dominio.Entities.Moto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Ano")
                        .HasColumnType("integer");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("boolean");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("char(7)");

                    b.HasKey("Id");

                    b.HasIndex("Placa")
                        .IsUnique();

                    b.ToTable("Moto");
                });

            modelBuilder.Entity("Dominio.Entities.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Criado")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("EntregadorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Situacao")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<decimal>("ValorDaCorrida")
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("Notificacoes", b =>
                {
                    b.Property<Guid>("NotificacoesId")
                        .HasColumnType("uuid")
                        .HasColumnName("PedidoId");

                    b.Property<Guid>("NotificadosId")
                        .HasColumnType("uuid")
                        .HasColumnName("EntregadorId");

                    b.HasKey("NotificacoesId", "NotificadosId");

                    b.HasIndex("NotificadosId");

                    b.ToTable("Notificacoes");
                });

            modelBuilder.Entity("Dominio.Entities.Locacao", b =>
                {
                    b.HasOne("Dominio.Entities.Entregador", "Entregador")
                        .WithMany("Locacoes")
                        .HasForeignKey("EntregadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Entities.Moto", "Moto")
                        .WithMany("Locacoes")
                        .HasForeignKey("MotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entregador");

                    b.Navigation("Moto");
                });

            modelBuilder.Entity("Dominio.Entities.Pedido", b =>
                {
                    b.HasOne("Dominio.Entities.Entregador", "Entregador")
                        .WithMany("Pedidos")
                        .HasForeignKey("EntregadorId");

                    b.Navigation("Entregador");
                });

            modelBuilder.Entity("Notificacoes", b =>
                {
                    b.HasOne("Dominio.Entities.Pedido", null)
                        .WithMany()
                        .HasForeignKey("NotificacoesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Entities.Entregador", null)
                        .WithMany()
                        .HasForeignKey("NotificadosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.Entities.Entregador", b =>
                {
                    b.Navigation("Locacoes");

                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("Dominio.Entities.Moto", b =>
                {
                    b.Navigation("Locacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
