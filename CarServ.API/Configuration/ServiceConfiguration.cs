using CarServ.service.Services.Interfaces;
using CarServ.service.Services;
using CarServ.service.WorkerService;
using CarServ.Repository.Repositories;

namespace CarServ.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPartServices, PartServices>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPackageServices, PackageServices>();
            services.AddScoped<INotificationervice, Notificationervice>();
            services.AddScoped<IPaymentervice, Paymentervice>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IAppointmentervices, Appointmentervices>();
            services.AddScoped<IWarrantyClaimervice, WarrantyClaimervice>();
            services.AddScoped<AdminSeederService>();
            

            return services;
        }
    }
}
