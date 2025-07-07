using CarServ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;


namespace CarServ.API.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string 'DefaultConnection' is not configured.");
            }
            services.AddDbContext<CarServicesManagementSystemContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
