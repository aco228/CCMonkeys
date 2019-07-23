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
      Test();
      

      string testasdasd = @"DAXKMA\".EscapeString();

      SortedDictionary<int, string> test = new SortedDictionary<int, string>();
      test.Add(8, "a=");
      test.Add(3, "a=");
      test.Add(12, "a=");
      test.Add(1, "a=");
      test.Add(125, "a=");
      test.Add(16, "a=");

      string encrypt = Crypter.Encrypt(8.ToString());
      int a = 0;
      string decrypt = Crypter.Decrypt(encrypt + "asdkhas");
      int akdjas = 0;


      //LeadDM lead = db.Query<LeadDM>().Load(15);
      //lead.TryUpdateEmail(db, "testststst@ggmail.com");
      //lead.TryUpdateMsisdn(db, "2454s64654");
      //lead.TryUpdateAddress(db, "2454646f54");
      //lead.UpdateLater();
      //db.TransactionalManager.RunAsync();


      //Console.ReadKey();
      //return;

      //db.ModelsCreator.GenerateFile("tm_action_account", "ActionAccount", @"D:\github\CCMonkeys\_rest\output");
      Console.ReadKey();
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
