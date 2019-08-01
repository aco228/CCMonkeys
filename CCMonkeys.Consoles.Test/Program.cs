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
      Test();
      return;

    }


    public static async void Test()
    {
      CCSubmitDirect db = CCSubmitDirect.Instance;
      var lead = await LeadDM.LoadByMsisdnOrEmailAsync(db, "61405016390", "fine_richard@hotmail.com");

      int a = 0;
    }


  }
}
