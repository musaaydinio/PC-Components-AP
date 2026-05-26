using Microsoft.EntityFrameworkCore;
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
        public static void ConfigureSqlContex(this IServiceCollection services,IConfiguration configuration)
        =>services.AddDbContext<StoreDbcontex>(options => options.UseSqlServer(configuration
                .GetConnectionString("sqlConnection")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServicesManager>();
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerServices, LoggerManager>();
    }
}
