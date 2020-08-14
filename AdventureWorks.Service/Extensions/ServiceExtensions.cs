using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Repositories;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Service.Extensions;
using AdventureWorks.Service.Filters;
using LoggerService;

namespace AdventureWorks.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMvcCore(this IServiceCollection services, IHostingEnvironment env)
        {
            services.AddMvcCore(config => config.Filters.Add(new AdventureWorksExceptionFilter(env)))
                .AddJsonFormatters(j =>
                {
                    j.ContractResolver = new DefaultContractResolver();
                    j.Formatting = Formatting.Indented;
                });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://localhost:5000", "https://localhost:5001")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        public static void ConfigureSqlServerContext(this IServiceCollection services, string connStr)
        {
            services.AddDbContextPool<AdventureWorksContext>(
                options => options.UseSqlServer(connStr)
            );
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "AdventureWorks Service",
                        Version = "v1",
                        Description = "Service to support the AdventureWorks ERP site",
                        TermsOfService = "None",
                        License = new License
                        {
                            Name = "Freeware",
                            Url = "http://localhost:23741/LICENSE.txt"
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IVendorRepo, VendorRepo>();
        }
    }
}