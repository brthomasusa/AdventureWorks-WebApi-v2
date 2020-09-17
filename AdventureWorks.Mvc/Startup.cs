using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NLog;

using AdventureWorks.Dal.EfCode;
using AdventureWorks.Mvc.Infrastructure;
using AdventureWorks.Dal.Repositories.Base;
using LoggerService;

namespace AdventureWorks.Mvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddDbContextPool<AdventureWorksContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:AdventureWorks_Testing"])
            );
            services.AddCloudscribePagination();
            services.AddTransient<IRepositoryCollection, RepositoryCollection>();

            services.AddMvc(config =>
            {
                config.Filters.Add(new DbExceptionAttribute(new LoggerManager(), HostingEnvironment));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
