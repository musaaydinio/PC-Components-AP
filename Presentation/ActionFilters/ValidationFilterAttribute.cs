using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    // Controller metotlarına gelen DTO parametrelerini ve model doğruluğunu (ModelState) kontrol ettiğimiz filter sınıfımız.
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        // İstek henüz action metoduna ulaşmadan hemen önce doğrulama kontrollerini yapıyoruz.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var contorller = context.RouteData.Values["controller"];
            var action=context.RouteData.Values["action"];

            // Metoda gönderilen DTO parametresini yakalıyoruz.
            var param =context.ActionArguments
                .SingleOrDefault(p=>p.Value.ToString().Contains("Dto")).Value;

            // Parametre null geldiyse işlemi kesip BadRequest (400) dönüyoruz.
            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null." +
                    $"Controller : {contorller}" +
                    $"Action : {action}");
                return;
            }
            // Model doğrulama kurallarına (Validation Attributes) uyulmadıysa UnprocessableEntity (422) yanıtı dönüyoruz.
            if (!context.ModelState.IsValid)
                context.Result=new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
}
