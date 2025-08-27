using Application.Interfaces;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repositories;

using Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default"));

// Application
builder.Services.AddScoped<IVentaService, VentaService>();

// Infrastructure
builder.Services.AddScoped<IVentaRepository, VentaRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();