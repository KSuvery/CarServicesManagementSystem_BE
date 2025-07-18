﻿using CarServ.Service.Services.Interfaces;
using CarServ.Service.Services;
using CarServ.Service.WorkerService;
using CarServ.Repository.Repositories;

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
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IWarrantyClaimService, WarrantyClaimService>();
            services.AddScoped<AdminSeederService>();
            

            return services;
        }
    }
}
