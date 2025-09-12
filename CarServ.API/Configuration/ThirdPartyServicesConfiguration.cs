using CarServ.service.Services.Configuration;
using Microsoft.Extensions.Options;

namespace CarServ.API.Configuration
{
    public static class ThirdPartyServicesConfiguration
    {
        public static IServiceCollection AddThirdPartyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ThirdPartyServicesCollection(configuration);
            return services;
        }

        public static void ThirdPartyServicesCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VnPaySetting>(options =>
            {
                options.TmnCode = configuration["VnPay:TmnCode"];
                options.HashSecret = configuration["VnPay:HashSecret"];
                options.BaseUrl = configuration["VnPay:BaseUrl"];
                options.Version = configuration["VnPay:Version"];
                options.CurrCode = configuration["VnPay:CurrCode"];
                options.Locale = configuration["VnPay:Locale"];
            });
            VnPaySetting.Instance = services.BuildServiceProvider().GetService<IOptions<VnPaySetting>>().Value;

            services.Configure<CloudinarySetting>(options =>
            {
                options.CloudinaryUrl = configuration["Cloudinary:CloudinaryUrl"];
            });
            CloudinarySetting.Instance = services.BuildServiceProvider().GetService<IOptions<CloudinarySetting>>().Value;
        }
    }
}
