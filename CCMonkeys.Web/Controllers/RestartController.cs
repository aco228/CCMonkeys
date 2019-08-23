using CCMonkeys.Web.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  public class RestartController : Controller
  {

    public IActionResult Index()
    {
      CacheManager.Init(true);
      return this.Content("OK (cm initiated)!");
    }

  }
}
