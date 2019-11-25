using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code.Filters;
using CCMonkeys.Web.Core.Controllers;
using Direct;
using Direct.ccmonkeys.Models;
using Direct.Models;
using Direct.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers.Direct
{
  [DashboardLoginAttribute]
  public abstract class DirectControllerBase<T> : DirectWebController<T>
    where T : DirectModel
  {
    public override DirectDatabaseBase Database => CCSubmitDirect.Instance;
    protected MainContext MainContext => new MainContext(this.HttpContext);

    protected override bool HasPrivilegesForSelect()
    {
      if (this.MainContext.Admin == null)
        return false;

      return true;
    }

    protected override bool HasPrivilegesForUpdate()
    {
      if (this.MainContext.Admin == null)
        return false;

      if (this.MainContext.Admin.GetStatus() != AdminStatusDM.Admin && 
        this.MainContext.Admin.GetStatus() != AdminStatusDM.Marketing)
        return false;

      return true;
    }

    protected override bool HasPrivilegesForInsert()
    {
      if (this.MainContext.Admin == null)
        return false;

      if (this.MainContext.Admin.GetStatus() != AdminStatusDM.Admin &&
        this.MainContext.Admin.GetStatus() != AdminStatusDM.Marketing)
        return false;

      return true;
    }

  }
}
