using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.Filters {
    public class ValidateModel : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext context) {
            base.OnActionExecuted(context);
            if (!context.ModelState.IsValid) {
                context.Result = new BadRequestResult();
            }
        }
    }
}