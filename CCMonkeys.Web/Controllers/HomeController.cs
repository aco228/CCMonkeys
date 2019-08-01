using CCMonkeys.Loggings;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Microsoft.AspNetCore.Mvc;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  public class HomeController : ControllerBase
  {

    public IActionResult Index()
    {
      return this.Content("OK!");
    }

    public IActionResult Test()
    {
      HomeController a = null;
      PrelanderTagModel testModel = new PrelanderTagModel()
      {
        answer = "answer", index = 8, tag = "tag"
      };

      try
      {
        string aasd = a.ControllerContext.ToString();
      }
      catch(Exception e)
      {
        Logger.Instance.StartLoggin("key_of_this_shit")
          .Where("HomeController")
          .Add("val1", "res1")
          .Add("val2", "res1")
          .Add(testModel)
          .OnException(e);
      }

      return this.Content("OK! ");
    }

  }
}
