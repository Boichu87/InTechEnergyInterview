using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Students;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AcademiaDbContext>(
    opt => opt.UseSqlServer(config.GetConnectionString("Default")));
builder.Services.AddDbContext<StudentsDbContext>(
    opt => opt.UseSqlServer(config.GetConnectionString("Default")));
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
