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
            });
            services.AddHangfireServer();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                AppPath = "#",//返回时跳转的地址
                DisplayStorageConnectionString = true,//是否显示数据库连接信息
                IsReadOnlyFunc = Context => false,
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
            app.UseMvc();


            RecurringJob.AddOrUpdate("订单取消", () => OrderJob.OrderCancelTask(), ConfigUtil.OrderCancelCron);

            if (ConfigUtil.OrderReturnEnabled)
                RecurringJob.AddOrUpdate("订单退款", () => OrderJob.OrderReturnTask(), Cron.Daily(ConfigUtil.OrderReturnTime));
        }
    }
}
