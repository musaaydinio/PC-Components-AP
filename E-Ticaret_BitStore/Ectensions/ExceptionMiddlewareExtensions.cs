using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace E_Ticaret_BitStore.Ectensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app,ILoggerServices logger)
        {
            app.UseExceptionHandler(appErr =>
            {
                appErr.Run(async contex =>
                {
                    contex.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    contex.Response.ContentType = "application/json";

                    var contexFeature=contex.Features.Get<IExceptionHandlerFeature>();
                    if(contexFeature is null)
                    {
                        logger.LogError($"Something went wrog:{contexFeature.Error}");
                        await contex.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode=contex.Response.StatusCode,
                            Message="Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
