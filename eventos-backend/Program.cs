using eventos_backend.Data;
using Microsoft.EntityFrameworkCore;

var corsOrigins = "_corsOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});

var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
builder.Services.AddDbContext<EventosDataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
