using AspNetCoreRateLimit;
using E_Ticaret_BitStore.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Services.Contracts;
using Story.EF_Core;

var builder = WebApplication.CreateBuilder(args);

// NLog yapýlandýrma dosyamýzý okuyarak log sistemini baþlatýyoruz.
LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Controller servislerimizi; içerik pazarlýðý, XML desteði ve JSON dairesel referans çözümleriyle birlikte ekliyoruz.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
})
.AddNewtonsoftJson(opt =>
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.AddXmlDataContractSerializerFormatters()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Kendi özel ValidationFilter yapýmýzý kullandýðýmýz için varsayýlan model doðrulama filtresini pasife alýyoruz.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.ConfigureSwagger();
builder.Services.ConfigureSqlContex(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilter();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataSahper();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ResgiterRepositories();
builder.Services.ResgiterServices();
builder.Services.ConfigureHealthChecks(builder.Configuration);


var app = builder.Build();

// Global hata yakalama mekanizmamýzý uygulamaya dahil ediyoruz.
var logger =app.Services.GetRequiredService<ILoggerServices>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/V1/swagger.json", "MuNi Gaming V1");
        s.SwaggerEndpoint("/swagger/V2/swagger.json", "MuNi GamingV2");

        // JavaScript kodumuzu Swagger'ýn baþlýðýna (Head) enjekte ediyoruz
        s.HeadContent = @"
    <script>
        window.addEventListener('load', function() {
            // Tarayýcýnýn 'fetch' (istek atma) motorunu kopyalýyoruz
            const originalFetch = window.fetch;
            
            // Ýstek motorunu kendi ajanýmýzla deðiþtiriyoruz
            window.fetch = async function() {
                // Ýstek normal þekilde sunucuya gitsin
                const response = await originalFetch.apply(this, arguments);
                const url = arguments[0];
                
                // Giden isteðin URL'sinde 'login' kelimesi geçiyorsa ve iþlem baþarýlýysa (200)
                if (url && url.toLowerCase().includes('login') && response.status === 200) {
                    
                    // Gelen yanýtý kopyalayýp içindeki JSON verisini okuyoruz
                    response.clone().json().then(data => {
                        
                        // Senin API'nden dönen token'ý yakalýyoruz (Ýsmine göre accessToken da olabilir)
                        const token = data.token || data.accessToken;
                        
                        if (token) {
                            // Swagger'ýn kendi iç güvenlik mekanizmasýný (ui.authActions) çaðýrýp,
                            // kilit kutusuna Token'ý otomatik olarak yerleþtiriyoruz.
                            if (window.ui) {
                                window.ui.authActions.authorize({
                                    Bearer: {
                                        name: 'Bearer',
                                        schema: { type: 'http', scheme: 'bearer', bearerFormat: 'JWT' },
                                        value: token
                                    }
                                });
                                console.log('Sistem: Token havada yakalandý ve Swagger yetkilendirildi!');
                            }
                        }
                    });
                }
                return response;
            };
        });
    </script>";
    });
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// HTTP isteklerinin geçeceði güvenlik, önbellekleme ve yetkilendirme katmanlarýný sýrasýyla çalýþtýrýyoruz
app.UseIpRateLimiting();
app.UseCors("CorsPlay");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.ToString(),
            entries = report.Entries.Select(e => new
            {
                key = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.ToString(),
                exception = e.Value.Exception?.Message
            })
        };

        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapControllers();
app.ConfigureAndMigrateDatabase();

app.Run();
