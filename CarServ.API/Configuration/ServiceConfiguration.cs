using CarServ.Service.Services.Interfaces;
using CarServ.Service.Services;
using CarServ.Service;

namespace CarServ.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAppointmentServices, AppointmentServices>();
            services.AddScoped<IInventoryServices, InventoryServices>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPackageServices, PackageServices>();
            return services;
        }
    }
}
