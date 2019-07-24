using CCMonkeys.Direct;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.IP2ID;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Direct.Core.Bulk;
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
