using CCMonkeys.Direct;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.IP2ID;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCMonkeys.Consoles.Test
{
  class Program
  {
    

    static void Main(string[] args)
    {
      CCSubmitDirect db = new CCSubmitDirect();
      DateTime c = DateTime.Now;

      var dc = db.LoadContainer(@"SELECT * FROM ccmonkeys.tm_action AS a
        LEFT OUTER JOIN ccmonkeys.tm_user AS u ON a.userid=u.userid
        LEFT OUTER JOIN ccmonkeys.tm_lead AS l ON u.leadid=l.leadid
        WHERE a.actionid='ACTbc8f897328a74ea6bf17b8c43017df22';");

      double ms = (DateTime.Now - c).TotalMilliseconds;
      return;

    }

    public static async void Test()
    {
      var lead = await new LeadDM(new CCSubmitDirect())
      {
        countryid = 149,
        msisdn = "alskdjalksd",
        email = "654646546"
      }.InsertAsync<LeadDM>();
    }


  }
}
