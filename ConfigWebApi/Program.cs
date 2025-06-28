using ConfigurationLibrary.Models;
using ConfigurationLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace ConfigWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // EF Core DbContext
            builder.Services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ConfigurationReader Singleton (örnek SERVICE-A)
            builder.Services.AddSingleton(sp =>
            {
                var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
                return new ConfigurationReader("SERVICE-A", connStr, 60000);
            });

            // Swagger
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
        }
    }
}
