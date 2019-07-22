using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Direct.Core.Bulk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Consoles.Migration
{
  class Program
  {
    public static CCSubmitDirect Database = CCSubmitDirect.Instance;
    public static LivesportsDatabase livesportsDb = new LivesportsDatabase();
    public static Dictionary<string, LanderDM> Landers = new Dictionary<string, LanderDM>();
    public static Runner Runner = new Runner();
    public static DirectBulker Bulker = new DirectBulker(Database);

    public static string CacheFile = "";
    public static object QueryLock = new object();
    public static List<string> Queries = new List<string>();

    static void Main(string[] args)
    {
      Database.PreventDispose = true;
      CacheFile = "cache_" + Guid.NewGuid().ToString() + ".txt";

      Start();
    }

    static async void Start()
    {
      var landers = await Database.Query<LanderDM>().LoadAsync();
      foreach (var lander in landers)
        Landers.Add(lander.name, lander);

      int lastIndex = 0;
      //string txtFromFile = File.ReadAllText(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\lastIndex.txt");
      //int.TryParse(txtFromFile, out lastIndex);
      int? totalNumber = await livesportsDb.LoadIntAsync("SELECT COUNT(*) FROM [].cc_client;");
      Console.WriteLine("Starting..");

      DateTime started = DateTime.Now;
      List<int> numberOfInsertsPer10Seconds = new List<int>();
      numberOfInsertsPer10Seconds.Add(0);
      int numberOfInsertsPer10Seconds_currentIndex = 0;
      DateTime compareDate = DateTime.Now;

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
            LIMIT 800;", lastIndex)).ConvertList<ClientDM>();

        if (clients.Count == 0)
          break;

        foreach(var client in clients)
        {
          lastIndex = client.clientid;
          Runner.Run(client);
        }

        if((DateTime.Now - compareDate).TotalSeconds >= 10)
        {
          compareDate = DateTime.Now;
          numberOfInsertsPer10Seconds.Add(0);
          numberOfInsertsPer10Seconds_currentIndex++;
        }
        numberOfInsertsPer10Seconds[numberOfInsertsPer10Seconds_currentIndex] += clients.Count;

        Bulker.Run();

        Console.WriteLine($" - {lastIndex} / {totalNumber.Value} - ({GetMiddleValue(numberOfInsertsPer10Seconds)} inserts in ten seconds)");
        File.WriteAllText(@"D:\github\CCMonkeys\CCMonkeys.Consoles.Migration\bin\Debug\netcoreapp2.1\lastIndex.txt", lastIndex.ToString());
      }


      Bulker.RunAndWait();

      Console.WriteLine("");
      Console.WriteLine("");
      Console.WriteLine("Rows inserted in database: " + Bulker.RowsInserted);
      Console.WriteLine("Rows from livesports: " + totalNumber.Value);
      Console.WriteLine("Finished in " + (DateTime.Now - started).TotalSeconds);

      Console.WriteLine("");
      Console.WriteLine("Starting to finish auto increments for lead");

      Dictionary<int, string> updateQueries = new Dictionary<int, string>();
      foreach(var l in Runner.EmailCache)
      {
        if (l.Value.actions_count == 1) continue;
        if (!updateQueries.ContainsKey(l.Value.actions_count))
          updateQueries.Add(l.Value.actions_count, string.Format("UPDATE ccmonkeys.tm_lead SET actions_count={0} WHERE leadid IN (0", l.Value.actions_count));
        updateQueries[l.Value.actions_count] += "," + l.Value.ID;
      }

      int alsdkha = 0;

    }

    public static double GetMiddleValue(List<int> values)
    {
      int all = 0;
      foreach (var i in values) all += i;
      return all / values.Count;
    }

    public static void WriteToCache(ClientDM client)
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
