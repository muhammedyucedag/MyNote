using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyNoteSampleApp.Core.Filters
{
    public class AdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString(Constants.UserRole).ToLower() != "admin" )
            {
                context.Result = new RedirectToActionResult("Unauthorize", "Home", null);
            }
        }
    }
}
