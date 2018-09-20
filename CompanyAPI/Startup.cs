using CompanyAPI.Interfaces;
using CompanyAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TobitLogger.Core;
using TobitLogger.Logstash;
using TobitLogger.Middleware;
using TobitWebApiExtensions.Extensions;

namespace CompanyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // The HttpContextAccessor is required by RequestGuidContextProvider implementation
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add the shipped implementation itself
            services.AddSingleton<ILogContextProvider, RequestGuidContextProvider>();

            services.AddChaynsToken();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<ICompanyRepo, CompanyRepo>();
            //services.AddScoped<IAddressRepo, AddressRepo>();
           // services.AddScoped<IAddressRepo, EmployeeRepo>();
           // services.AddScoped<IAddressRepo, DepartmentRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILogContextProvider logContextProvider)
        {
            loggerFactory.AddLogstashLogger(Configuration.GetSection("Logger"), logContextProvider: logContextProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
                //app.UseHsts();
            }

            app.UseRequestLogging();
            app.UseAuthentication();

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //   // await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
