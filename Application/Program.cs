using Application.Configuration;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDepencyInjections(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();

app.UseAuthorization();

app.MapControllers();

app.Services.ExecuteMigration();

app.Run();