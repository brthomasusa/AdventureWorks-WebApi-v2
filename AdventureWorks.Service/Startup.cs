﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using NLog;

using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Repositories;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Service.Extensions;
using AdventureWorks.Service.Filters;
using LoggerService;

namespace AdventureWorks.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.ConfigureMvcCore(_env);
            services.ConfigureCors();
            services.ConfigureSqlServerContext(Configuration.GetConnectionString("AdventureWorks_Testing"));
            services.ConfigureSwagger();

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            services.AddScoped<IBusinessEntityAddressRepo, BusinessEntityAddressRepo>();
            services.AddScoped<IVendorRepo, VendorRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AdventureWorksContext>();
                    SampleDataInitialization.InitializeData(context);
                }
            }

            app.UseSwagger();

            app.UseSwaggerUI(
                c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdventureWorks-API Service v1"); }
            );

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseMvc();
        }
    }
}
