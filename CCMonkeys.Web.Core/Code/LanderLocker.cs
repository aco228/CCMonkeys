using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code
{
  public static class LanderLocker
  {

    public static bool HasPrelanderLocker(CCSubmitDirect db, DomainManager domainManager)
    {
      if (domainManager.Queries.ContainsKey("dbg"))
        return true;

      if (!domainManager.Queries.ContainsKey("msisdn") || string.IsNullOrEmpty(domainManager.Queries["msisdn"]))
        return false;

      string lockerID = string.Format("{0}_{1}", domainManager.Prelander.ID, domainManager.Queries["msisdn"]);
      int? result = db.LoadInt("SELECT COUNT(*) FROM [].tm_locker WHERE lockerid={0}", lockerID);
      if (result.HasValue && result.Value > 0)
        return false;

      db.Execute("INSERT INTO [].tm_locker (lockerid)", lockerID);
      return true;
    }

  }
}
