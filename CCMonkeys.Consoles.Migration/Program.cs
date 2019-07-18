using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Direct.Core.Bulk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CCMonkeys.Consoles.Migration
{
  class Program
  {
    static CCSubmitDirect Database = new CCSubmitDirect();
    static LivesportsDatabase livesportsDb = new LivesportsDatabase();
    static Dictionary<string, string> EmailCache = new Dictionary<string, string>();
    static Dictionary<string, string> MsisdnCache = new Dictionary<string, string>();
    static Dictionary<string, LanderDM> Landers = new Dictionary<string, LanderDM>();

    static string CacheFile = "";
    static object QueryLock = new object();
    static List<string> Queries = new List<string>();

    static void Main(string[] args)
    {
      CacheFile = "cache_" + Guid.NewGuid().ToString() + ".txt";

      Start();
    }

    static async void Start()
    {
      Database.PreventDispose = true;
      Database.TransactionalManager.Limit = 500000000;
      var landers = await Database.Query<LanderDM>().LoadAsync();
      foreach (var lander in landers)
        Landers.Add(lander.name, lander);


      var bulkManager = new BulkManager(Database, @"C:\Users\aco228_\Desktop\output\sql");


      int lastIndex = 0;
      string txtFromFile = File.ReadAllText(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\lastIndex.txt");
      int.TryParse(txtFromFile, out lastIndex);
      int? totalNumber = await livesportsDb.LoadIntAsync("SELECT COUNT(*) FROM [].cc_client;");
      Console.WriteLine("Starting..");

      for (; ; )
      {
        string queries = "";
        List<ClientDM> clients = (await livesportsDb.LoadContainerAsync(
          @"SELECT 
              clientid,
	            clickid , affid, payment_provider, pubid, 
                email, msisdn, firstname, lastname, country, referrer, address,
                username, password,
                has_subscription, has_chargeback, has_refund, times_charged, times_upsell, is_stolen,
                created
            FROM cc_client 
            WHERE clientid>{0} 
            LIMIT 1000;", lastIndex)).ConvertList<ClientDM>();

        foreach(var client in clients)
        {
          lastIndex = client.clientid;
          if (EmailCache.ContainsKey(client.email) || MsisdnCache.ContainsKey(client.msisdn))
          {
            WriteToCache(client);
            continue;
          }


          if(!string.IsNullOrEmpty(client.email))
            EmailCache.Add(client.email, string.Empty);
          if (!string.IsNullOrEmpty(client.msisdn))
            MsisdnCache.Add(client.msisdn, string.Empty);

          int? countryID = await CountryManager.GetCountryByName(Database, client.country);
          if (!countryID.HasValue)
            continue;

          if(string.IsNullOrEmpty(client.referrer)){ WriteToCache(client); continue; }

          string[] refSplit = client.referrer.Split('/');
          if(refSplit.Length < 3 ) { WriteToCache(client); continue; }

          string landingName = refSplit[3];
          if(!Landers.ContainsKey(landingName)) { WriteToCache(client); continue; }

          /// INSERT LOGIC

          var instance = new BulkInstance();
          var leadDM = new LeadDM(Database)
          {
            email = client.email, msisdn = client.msisdn,
            first_name = client.firstname, last_name = client.lastname,
            countryid = countryID, address = client.address, actions_count = 1
          }
          .ToBulkModel()
          .ChainTo(instance);
          
          var userDM = new UserDM(Database)
          {
            guid = Guid.NewGuid().ToString(),
            countryid = countryID
          }
          .ToBulkModel()
          .Link(leadDM)
          .ChainTo(instance);

          var actionDM = new ActionDM(Database)
          {
            guid = Guid.NewGuid().ToString(), trackingid = client.clickid,
            affid = client.affid, pubid = client.pubid, 
            landerid = Landers[landingName].ID, landertypeid = Landers[landingName].landertypeid,
            providerid = client.payment_provider,
            countryid = countryID,
            input_redirect = true, input_email = (!string.IsNullOrEmpty(client.email)), input_contact = (!string.IsNullOrEmpty(client.address)),
            has_subscription = client.has_subscription, has_refund = client.has_refund, has_chargeback = client.has_chargeback,
            times_charged = client.times_charged, times_upsell = client.times_upsell
          }
          .ToBulkModel()
          .Link(leadDM, userDM)
          .ChainTo(instance);

          bulkManager.Add(instance);
        }

        await bulkManager.Iteration();
        Task.Factory.StartNew( ()=> {
          (new CCSubmitDirect()).ExecuteAsync(File.ReadAllText(bulkManager.OutputFile));
        });
        System.Threading.Thread.Sleep(1500);
        bulkManager.ResetOutputIteration();

        //Database.TransactionalManager.WriteInsertIntoExternalFile(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\insert.sql");

        Console.WriteLine($"{lastIndex}/{totalNumber.Value}");
        File.WriteAllText(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\lastIndex.txt", lastIndex.ToString());
      }

    }

    static void WriteToCache(ClientDM client)
    {
      string line = client.clientid.ToString() + "#"
        + client.clickid.ToString() + "#"
        + client.affid.ToString() + "#"
        + client.payment_provider.ToString() + "#"
        + client.pubid.ToString() + "#"
        + client.email.ToString() + "#"
        + client.msisdn.ToString() + "#"
        + client.firstname.ToString() + "#"
        + client.lastname.ToString() + "#"
        + client.country.ToString() + "#"
        + client.referrer.ToString() + "#"
        + client.address.ToString() + "#"
        + client.username.ToString() + "#"
        + client.password.ToString() + "#"
        + client.has_subscription.ToString() + "#"
        + client.has_chargeback.ToString() + "#"
        + client.has_refund.ToString() + "#"
        + client.times_charged.ToString() + "#"
        + client.times_upsell.ToString() + "#"
        + client.is_stolen.ToString() + "#"
        + client.created.ToString() + Environment.NewLine;
      File.AppendAllText(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\caches\" + CacheFile, line);
    }
  }
}
