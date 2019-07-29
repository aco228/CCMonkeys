using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.Filters
{
  public class AllowCrossSiteAttribute : ActionFilterAttribute
  {

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      #if DEBUG
        filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
        filterContext.HttpContext.Response.Headers.Add("Vary", "Origin");
      #else
      #endif

      base.OnActionExecuting(filterContext);
    }
  }
}
