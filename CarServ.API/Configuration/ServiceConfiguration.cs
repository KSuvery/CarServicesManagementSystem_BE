using CarServ.service.Services.Interfaces;
using CarServ.service.Services;
using CarServ.service.WorkerService;
using CarServ.Repository.Repositories;
using CarServ.Service.Services.Interfaces;
using CarServ.Service.Services;
using CarServ.Repository.Repositories.Interfaces;

namespace CarServ.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPartsService, PartsService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPackageServices, PackageServices>();
            services.AddScoped<INotificationervice, Notificationervice>();
            services.AddScoped<IPaymentService, Paymentervice>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IVnPayService, VnPayService>();            
            services.AddScoped<IAppointmentervices, Appointmentervices>();
            services.AddScoped<IWarrantyClaimervice, WarrantyClaimervice>();
            services.AddScoped<AdminSeederService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ICarTypesService, CarTypesService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
