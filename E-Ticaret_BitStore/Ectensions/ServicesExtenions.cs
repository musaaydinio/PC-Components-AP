using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repository.Contracts;
using Repository.EF_Core;
using Services;
using Services.Contracts;
using Story.EF_Core;
using System.Runtime.CompilerServices;

namespace E_Ticaret_BitStore.Ectensions
{
    public static class ServicesExtenions
    {
        public static void ConfigureSqlContex(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<StoreDbcontex>(options => options.UseSqlServer(configuration
                .GetConnectionString("sqlConnection"),
             b => b.MigrationsAssembly("Repository")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServicesManager>();
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerServices, LoggerManager>();

        public static void ConfigureActionFilter(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
                );
            });
        }

        public static void ConfigureDataSahper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<ProductDto>, DataShaper<ProductDto>>();
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                opt.Conventions.Controller<ProductController>()
                .HasDeprecatedApiVersion(new ApiVersion(1, 0));

                opt.Conventions.Controller<ProductV2Controller>()
               .HasDeprecatedApiVersion(new ApiVersion(2, 0));

            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(exprationopt =>
            {
                exprationopt.MaxAge = 90;
                exprationopt.CacheLocation = CacheLocation.Private;
            },
            validationopt =>
            {
                validationopt.MustRevalidate = false;
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint="*",
                    Limit =5,
                    Period="1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
    }
}


       
