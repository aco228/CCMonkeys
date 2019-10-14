using CCMonkeys.Direct;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LockerController : ControllerBase
  {

    [HttpGet("check/{value}")]
    public bool Check(string value)
    {
      CCSubmitDirect db = CCSubmitDirect.Instance;

      int? result = db.LoadInt("SELECT COUNT(*) FROM [].tm_locker WHERE lockerid={0}", value);
      if (result.HasValue && result.Value > 0)
        return false;

      db.Execute("INSERT INTO [].tm_locker (lockerid)", value);
      return true;
    }

  }
}
