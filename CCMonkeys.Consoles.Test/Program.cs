using CCMonkeys.Direct;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.IP2ID;
using Direct;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CCMonkeys.Consoles.Test
{
  class Program
  {

    //static string[] data = new string[]
    //{
    //  "a1", "a2", "aco je car"
    //};

    static void Main(string[] args)
    {

      Dictionary<string, bool> test = new Dictionary<string, bool>();
      test.Add("aasd", false);
      test.Add("aasd1", false);
      test.Add("aasd2", false);
      test.Add("aasd3", false);
      test.Add("aasd4", false);
      test.Add("aasd5", false);
      test.Add("aasd6", false);
      test.Add("aasd7", false);
      test.Add("aasd8", false);
      test.Add("aasd9", false);
      test.Add("aasd0", false);
      test.Add("aasd45", false);
      test.Add("aasd234", false);
      test.Add("aasdsdf", false);
      var a = DirectBinary.Serialize(test);


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
