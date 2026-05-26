using Entities.ErrorModel;
using Entities.Exceptions;
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
                    
                    contex.Response.ContentType = "application/json";
                    var contexFeature=contex.Features.Get<IExceptionHandlerFeature>();
                    if(contexFeature is not null)
                    {
                        contex.Response.StatusCode = contexFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        logger.LogError($"Something went wrog:{contexFeature.Error}");

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
