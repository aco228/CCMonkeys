using CCMonkeys.Direct;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.Legacy
{
  public abstract class LegacyBase
  {
    protected CCSubmitDirect Database = null;
    protected HttpContext Request = null;

    public LegacyBase(CCSubmitDirect db, HttpContext request)
    {
      this.Database = db;
      this.Request = request;
    }

    public abstract void Run();

  }
}
