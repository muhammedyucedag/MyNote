using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyNoteSampleApp.Core.Filters
{
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32(Constants.UserId).GetValueOrDefault() == 0)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }
}
