using System;
using System.Collections.Generic;
using System.Text;
using CCMonkeys.Web.Core.Code.Filters;
using Microsoft.AspNetCore.Hosting;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [DashboardLoginAttribute]
  public abstract class DashboardController : MainController
  {
    public DashboardController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }
  }
}
