using Microsoft.EntityFrameworkCore;
using Dominio.Entities;
using System.Reflection;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Infraestrutura.Context
{
    public  class AppDbContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options) 
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Postgre"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
