using CCMonkeys.Web.Core;
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
      try
      {
        string aasd = a.ControllerContext.ToString();
      }
      catch(Exception e)
      {
        Logger.Instance.LogException(e);
        var raven = new RavenClient("https://e2a9518558524ceeafd180cf83556583@sentry.io/1505328");
        raven.Capture(new SentryEvent(e));

        Logger.Instance.LogException(e);
      }

      var properties = new Dictionary<string, string>();
      properties.Add("test1", "test1va");
      properties.Add("test2", "test1va");
      properties.Add("test3", "test1va");
      properties.Add("test4", "test1va");
      int aadsasAleksandarKonatarasldkjalsdkjNovi = 0;
      //Logger.Instance.TrackEvent("evetnName", "username", properties);

      return this.Content("OK! ");
    }

  }
}
