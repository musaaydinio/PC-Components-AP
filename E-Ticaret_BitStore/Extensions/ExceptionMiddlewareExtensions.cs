using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace E_Ticaret_BitStore.Extensions
{
    // Uygulama genelinde oluşan hataları tek bir merkezden yakalayıp yönettiğimiz middleware extension sınıfımız.
    public static class ExceptionMiddlewareExtensions
    {
        // Global hata yakalama mekanizmasını pipeline'a dahil ettiğimiz extension metodumuz.
        public static void ConfigureExceptionHandler(this WebApplication app,ILoggerServices logger)
        {
            app.UseExceptionHandler(appErr =>
            {
                appErr.Run(async contex =>
                {
                    
                    contex.Response.ContentType = "application/json";
                    var contexFeature=contex.Features.Get<IExceptionHandlerFeature>();
                    if(contexFeature is not null)
                    {
                        // Fırlatılan hatanın tipine göre istemciye döneceğimiz HTTP durum kodunu belirliyoruz.
                        contex.Response.StatusCode = contexFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        // Yakaladığımız hatayı log servisimize kaydediyoruz.
                        logger.LogError($"Something went wrog:{contexFeature.Error}");

                        // Hata detaylarını JSON formatı olarak yanıtı istemciye dönüyoruz.
                        await contex.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode=contex.Response.StatusCode,
                            Message=contexFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
