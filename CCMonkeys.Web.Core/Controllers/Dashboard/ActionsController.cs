using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Models.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{


  [Route("api/")]
  public class ActionsController : MainController
  {
    public ActionsController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    [HttpGet("/")]
    public async Task<IActionResult> GetActions([FromQuery]ActionControllerModel input)
    {

      return this.Content("OK");
    }

  }
}
