using Microsoft.EntityFrameworkCore;
using ClamManagement.Data;
using ClamManagement.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
var connStr = "server=192.168.1.3;port=3306;user=user;password=123456;database=Clam;Convert Zero Datetime=True;";
builder.Services.AddDbContext<Context>(opt => opt.UseMySQL(
    connStr
  ));
builder.Services
    .AddRepositories()
    .AddServices()
    .AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
