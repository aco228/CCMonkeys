using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  [Route("fbid")]
  public class FacebookPostRedirectController : Controller
  {

    [HttpGet("{id}")]
    public IActionResult Index(string id)
    {
      ViewBag.ID = id;
      return View("~/Views/facebook_pixel.cshtml");
    }

  }
}
