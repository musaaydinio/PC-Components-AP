
using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILoggerServices loggerServices;

        public LogFilterAttribute(ILoggerServices logger)
        {
            loggerServices = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            loggerServices.LogInfo(Log("0nActionExecutting",context.RouteData));
        }

        private string Log(string modelName, RouteData routeData)
        {
            var LogDetails = new LogDetails()
            {
                ModelName = modelName,
                Contorller = routeData.Values["controller"],
                Action = routeData.Values["action"]
            };
            if(routeData.Values.Count >= 3)
            {
                LogDetails.Id=routeData.Values["Id"];
            }
            return LogDetails.ToString();
        }
    }   
}
