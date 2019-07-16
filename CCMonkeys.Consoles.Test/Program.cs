using CCMonkeys.Direct;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.IP2ID;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCMonkeys.Consoles.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      var db = new CCSubmitDirect();

      LeadDM lead = db.Query<LeadDM>().Load(15);
      lead.TryUpdateEmail(db, "testststst@ggmail.com");
      lead.TryUpdateMsisdn(db, "2454s64654");
      lead.TryUpdateAddress(db, "2454646f54");
      lead.UpdateLater();
      db.TransactionalManager.RunAsync();


      Console.ReadKey();
      return;

      db.ModelsCreator.GenerateFile("tm_session", "Session", @"D:\github\CCMonkeys\_rest\output");
      db.ModelsCreator.GenerateFile("tm_action", "Action", @"D:\github\CCMonkeys\_rest\output");
      Console.ReadKey();
      return;

    }

    public static void Test(ReceivingRegistrationModel model)
    {
      string domain = model.url.Split('?')[0];
      string query = string.Empty;
      if (model.url.Contains('?'))
        query = model.url.Split('?')[1];
      var queryValues = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);


      int a = 0;
    }


  }
}
