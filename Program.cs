using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UniversidadeContext>(opt =>
    opt.UseInMemoryDatabase("Universidade"));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UniversidadeContext>(opt =>
    opt.UseInMemoryDatabase("UniversidadeList"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
