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

          //  services.AddHangfireServer();

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Hangfire.HttpJob",
                    Version = "v1",
                    Description = "动态新增任务"
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
            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                AppPath = "#",//返回时跳转的地址
                DisplayStorageConnectionString = true,//是否显示数据库连接信息
                IsReadOnlyFunc = Context => false,
                Authorization = new[] { new CustomAuthorizeFilter() }
            });


            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseMvc();


            RecurringJob.AddOrUpdate("core订单取消", () => OrderJob.OrderCancelTask(), ConfigUtil.OrderCancelCron);

            if (ConfigUtil.OrderReturnEnabled)
                RecurringJob.AddOrUpdate("core订单退款", () => OrderJob.OrderReturnTask(), Cron.Daily(ConfigUtil.OrderReturnTime));
        }
    }
}
