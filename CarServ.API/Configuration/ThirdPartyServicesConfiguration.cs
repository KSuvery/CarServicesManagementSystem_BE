using CarServ.service.Services.Configuration;
using Microsoft.Extensions.Options;

namespace CarServ.API.Configuration
{
    public static class ThirdPartyServicesConfiguration
    {
        public static IServiceCollection AddThirdPartyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ThirdPartyServicesCollection();
            return services;
        }

        public static void ThirdPartyServicesCollection(this IServiceCollection services)
        {
            services.Configure<VnPaySetting>(options =>
            {
                options.TmnCode = GetEnvironmentVariableOrThrow("VNPAY_TMN_CODE");
                options.HashSecret = GetEnvironmentVariableOrThrow("VNPAY_HASH_SECRET");
                options.BaseUrl = GetEnvironmentVariableOrThrow("VNPAY_BASE_URL");
                options.Version = GetEnvironmentVariableOrThrow("VNPAY_VERSION");
                options.CurrCode = GetEnvironmentVariableOrThrow("VNPAY_CURR_CODE");
                options.Locale = GetEnvironmentVariableOrThrow("VNPAY_LOCALE");
            });
            VnPaySetting.Instance = services.BuildServiceProvider().GetService<IOptions<VnPaySetting>>().Value;

            services.Configure<CloudinarySetting>(options =>
            {
                options.CloudinaryUrl = GetEnvironmentVariableOrThrow("CLOUDINARY_URL");
            });
            CloudinarySetting.Instance = services.BuildServiceProvider().GetService<IOptions<CloudinarySetting>>().Value;
        }

        private static string GetEnvironmentVariableOrThrow(string key)
        {
            return Environment.GetEnvironmentVariable(key)
                   ?? throw new ArgumentNullException(key, $"Environment variable '{key}' is not set.");
        }
    }
}
