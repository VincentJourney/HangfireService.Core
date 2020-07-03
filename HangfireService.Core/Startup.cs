using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HangfireService.Business.CrmPlatForm.Job;
using HangfireService.Commom;
using Microsoft.OpenApi.Models;
using HangfireService.Core.Filter;
using Hangfire.Dashboard;

namespace HangfireService.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(ConfigUtil.ConnectionString);
                config.UseSerilogLogProvider();
                config.UseFilter<CustomJobFilter>(new CustomJobFilter());
            });

            services.AddHangfireServer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hangfire.HttpJob",
                    Version = "v1",
                    Description = "Extensions => HttpApi Operation Job",
                    Contact = new OpenApiContact
                    {
                        Name = "Vincent",
                        Email = "953567835@qq.com",
                        Url = new Uri("https://github.com/VincentJourney/HangfireService.Core")
                    }
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard("/Hangfire-Read", new DashboardOptions
            {
                AppPath = "#",
                DisplayStorageConnectionString = true,
                IsReadOnlyFunc = Context => true,
                Authorization = new[] { new CustomAuthorizeFilter(new List<AuthorizeUser>
                {
                   new AuthorizeUser{ UserName="admin",PassWord="123456" },
                   new AuthorizeUser{ UserName="aic",PassWord="123456" },
                }) }
            });

            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                AppPath = "#",
                DisplayStorageConnectionString = true,
                IsReadOnlyFunc = Context => false,
                Authorization = new[] { new CustomAuthorizeFilter(new List<AuthorizeUser>
                {
                   new AuthorizeUser{ UserName="admin",PassWord="123456" },
                   new AuthorizeUser{ UserName="aic",PassWord="123456" },
                }) }
            });


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
            //RecurringJob.AddOrUpdate("core订单取消", () => OrderJob.OrderCancelTask(), ConfigUtil.OrderCancelCron);
            //if (ConfigUtil.OrderReturnEnabled)
            //    RecurringJob.AddOrUpdate("core订单退款", () => OrderJob.OrderReturnTask(), Cron.Daily(ConfigUtil.OrderReturnTime));
        }
    }
}
