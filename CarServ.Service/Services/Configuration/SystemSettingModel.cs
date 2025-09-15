using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Configuration
{
    public class SystemSettingModel
    {
        private static SystemSettingModel _instance;
        public static IConfiguration Configuration { get;  set; }
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name; 
        public string? Domain { get; set; }

        public static SystemSettingModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SystemSettingModel();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }

    public class VnPaySetting
    {
        public static VnPaySetting Instance { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string CurrCode { get; set; }
        public string Locale { get; set; }
    }

    public class CloudinarySetting
    {
        public static CloudinarySetting Instance { get; set; }
        public string CloudinaryUrl { get; set; }
    }

    public class OpenAiSetting
    {
        public static OpenAiSetting Instance { get; set; }
        public string ApiKey { get; set; }
    }

    public class AzureOpenAiSetting
    {
        public static AzureOpenAiSetting Instance { get; set; }
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; }
        }
}
