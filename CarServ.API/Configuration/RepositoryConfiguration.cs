using CarServ.Repository.Repositories.Interfaces;
using CarServ.Repository.Repositories;

namespace CarServ.API.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IWarrantyClaimRepository, WarrantyClaimRepository>();
            return services;
        }
    }
}
