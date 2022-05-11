using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using SoftDelete.Test.Configurations;
using SoftDelete.Test.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SoftDeleteContext>(
    options => options.UseSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: options => options.MigrationsAssembly(typeof(Program).Namespace)));

builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration).Assembly);

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
