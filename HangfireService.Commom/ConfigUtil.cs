using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace HangfireService.Commom
{
    public class ConfigUtil
    {
        static IConfiguration configuration;
        static ConfigUtil()
        {
            configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }

        public static T GetConfig<T>(string key)
        {
            try
            {
                return configuration.GetValue<T>(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"【key:{key}】 | 【ex:{ex.Message}】");
            }
        }

        public static string ConnectionString { get => GetConfig<string>("ConnectionString"); }

        public static string CRMUrl { get => GetConfig<string>("CRMUrl"); }
        public static string CRMTokenAppid { get => GetConfig<string>("CRMTokenAppid"); }
        public static string CRMTokenSecret { get => GetConfig<string>("CRMTokenSecret"); }
        public static bool OrderReturnEnabled { get => GetConfig<bool>("OrderReturnEnabled"); }
        public static int OrderReturnTime { get => GetConfig<int>("OrderReturnTime"); }
        public static string OrderCancelCron { get => GetConfig<string>("OrderCancelCron"); }



    }
}
