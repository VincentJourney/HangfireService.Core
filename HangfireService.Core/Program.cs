using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Serilog;

namespace HangfireService.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //host.RunAsService();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(c =>
            {
                c.ClearProviders();
            })
            .UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext();
                config.MinimumLevel.Warning();
            })
            .UseStartup<Startup>();
    }
}
