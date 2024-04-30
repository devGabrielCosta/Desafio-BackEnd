using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Dominio.Services;
using Infraestrutura.Context;
using Infraestrutura.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IMotoService, MotoService>();

builder.Services.AddScoped<IEntregadorRepository, EntregadorRepository>();
builder.Services.AddScoped<IEntregadorService, EntregadorService>();

builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (_db != null)
    {
        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }
    }
}

app.Run();
