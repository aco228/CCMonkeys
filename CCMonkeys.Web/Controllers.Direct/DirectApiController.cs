using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers.Direct
{
  [Route("direct/api")]
  public class DirectApiController : ControllerBase
  {
    public IActionResult Index()
    {
      Assembly asm = Assembly.GetExecutingAssembly();
      var asmTypes = asm.GetTypes();

      var controllers = (from a in asmTypes where !string.IsNullOrEmpty(a.Namespace) && a.Namespace.Equals("CCMonkeys.Web.Controllers.Direct") select a).ToList();
      string response = string.Empty;
      foreach (var c in controllers)
      {
        var router = (from rc in c.CustomAttributes where rc.AttributeType.FullName.Equals("Microsoft.AspNetCore.Mvc.RouteAttribute") select rc).FirstOrDefault();
        if (router == null)
          continue;

        response += router.ConstructorArguments[0].ToString() + Environment.NewLine;
      }

      return this.Content(response);
    }
  }
}
