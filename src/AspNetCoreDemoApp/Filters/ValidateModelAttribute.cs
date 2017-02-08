using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreDemoApp
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public bool AllowNull { get; set; } = false;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!this.AllowNull && context.ActionArguments.Values.Any(value => value == null))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}