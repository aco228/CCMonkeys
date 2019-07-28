using CCMonkeys.Web.Core.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Code.Filters
{
  [AllowCrossSiteAttribute]
  public class DashboardLoginAttribute : ActionFilterAttribute
  {

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.HttpContext.Request.Cookies.ContainsKey(Constants.AdminCookie))
      {
        context.Result = new JsonResult(ModelBaseResponse.GenerateError("Credentials error!"));
        return;
      }
      else if(!context.HttpContext.Request.Headers.ContainsKey("authentication"))
      {
        context.Result = new JsonResult(ModelBaseResponse.GenerateError("Credentials error!"));
        return;
      }

      base.OnActionExecuting(context);
    }

  }
}
