using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repository.Contracts;
using Repository.EF_Core;
using Services;
using Services.Contracts;
using Story.EF_Core;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Ticaret_BitStore.Extensions
{
    // Program.cs dosyasını temiz tutmak için tüm servis kayıt ayarlarımızı bu extension sınıfında yapıyoruz.
    public static class ServicesExtenions
    {
        // SQL Server bağlantımızı ve Entity Framework Core'un migration işlemlerini hangi katmanda arayacağını belirtiyoruz.
        public static void ConfigureSqlContex(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<StoreDbcontex>(options => options.UseSqlServer(configuration
                .GetConnectionString("sqlConnection"),
             b => b.MigrationsAssembly("Repository")));

        // Repository pattern'i tek bir merkezden yönettiğimiz sınıfı sisteme tanıtıyoruz.
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        // Tüm iş kurallarımızı barındıran servislerimizi tek merkezden sunan sınıfı sisteme tanıtıyoruz.
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServicesManager>();

        // Uygulama genelinde log tutmamızı sağlayan NLog servisini Singleton olarak ekliyoruz.
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerServices, LoggerManager>();

        // Doğrulama (validation) ve loglama yapan filter'larımızı kaydediyoruz.
        public static void ConfigureActionFilter(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
        }

        // Farklı platformlardan  API'mize istek atılabilmesi için CORS izinlerini ayarlıyoruz.
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination") // İstemcinin sayfalama detaylarını header'dan okuyabilmesine izin veriyoruz.
                );
            });
        }

        // İstemcinin sadece istediği propertyleri çekebilmesini sağlayan Data Shaping servisimizi kaydediyoruz.
        public static void ConfigureDataSahper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<ProductDto>, DataShaper<ProductDto>>();
        }

        // API'mize versiyon desteği ekliyoruz. İstemci versiyon belirtmezse varsayılan olarak V1'i kabul ediyoruz.
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
        // Sık değişmeyen verilerde performansı artırmak için Response Caching mekanizmasını devreye alıyoruz.
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        // İstemci tarafında önbellekleme (Cache-Control) işlemlerini HTTP Header'ları üzerinden yönetmek için yapılandırıyoruz.
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
        // API'mizi gereksiz trafiğe ve basit DDoS ataklarına karşı korumak için IP bazlı istek sınırlandırması (Rate Limiting) kurallarını ayarlıyoruz.
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

        // Kullanıcı yetkilendirme (Identity) kütüphanesini, şifre zorluk kuralları ve benzersiz e-posta şartıyla projeye entegre ediyoruz.
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 7;

                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<StoreDbcontex>()
                .AddDefaultTokenProviders();
        }

        // JWT (JSON Web Token) ayarlarını, imza doğrulamasını ve token'ın geçerlilik parametrelerini yapılandırıyoruz.
        public static void ConfigureJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSetting");
            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                }
            );
        }
        // API'mizi arayüz üzerinden test edebilmek (Swagger) için dokümantasyon ayarlarını ve JWT Token giriş alanını (Bearer) ekliyoruz.
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(m =>
            {
                m.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "MuNi Gaming",
                    Version = "V1",
                    Contact = new OpenApiContact
                    {
                        Name = "Musa Aydın",
                        Email = "mfmy982@gmail.com",
                        Url = new Uri("https://github.com/musaaydinio")
                    }
                });
                m.SwaggerDoc("V2", new OpenApiInfo { Title = "MuNi Gaming", Version = "V2" });

                m.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                m.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Name="Bearer"
                    },
                    new List<string>()
                  }
                });
            });
        }

        // Veritabanı işlemlerini yapan repository sınıflarını sisteme tanımlıyoruz;
        // böylece ihtiyaç duyulan yerlerde 'new' demeden otomatik çağırıp kullanabiliyoruz.
        public static void ResgiterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepositroy, CateagoryRespository>();
        }
        // İş kurallarını yönettiğimiz servis sınıflarını sisteme tanıtıyoruz;
        // bu sayede controller taraflarında bu sınıfları doğrudan çağırabiliyoruz.
        public static void ResgiterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductServices, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IAuthenticationService, AuthenticationManager>();
        }

        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // Health Check servisini ekliyoruz ve SQL Server veritabanımızı kontrol etmesini söylüyoruz.
            services.AddDbContext<StoreDbcontex>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

            // 2. Health Check bağlantısı (Aynı metodun içinde!)
            services.AddHealthChecks()
                .AddDbContextCheck<StoreDbcontex>("Database Health Check");
        }
    }
}


       
