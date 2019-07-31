using CCMonkeys.Web.Core.Sockets.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  public class TestsController : ControllerBase
  {

    public IActionResult OnNewTransaction()
    {
      DashboardSocket.OnNewTransaction("providername", "actionid");
      return this.Content("ok");
    }
    public IActionResult OnNewChargeback()
    {
      DashboardSocket.OnNewChargeback("providername", "actionid");
      return this.Content("ok");
    }
    public IActionResult OnNewRefund()
    {
      DashboardSocket.OnNewRefund("providername", "actionid");
      return this.Content("ok");
    }
    public IActionResult OnNewUpsell()
    {
      DashboardSocket.OnNewUpsell("providername", "actionid");
      return this.Content("ok");
    }

  }
}
