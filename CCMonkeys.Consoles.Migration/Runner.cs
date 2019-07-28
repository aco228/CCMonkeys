using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Bulk;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Consoles.Migration
{

  public class LeadUser
  {
    public int ID;
    public int actions_count = 1;
  }

  public class Runner
  {
    static int CurrentIndex = 1;
    public static Dictionary<string, LeadUser> EmailCache = new Dictionary<string, LeadUser>();
    public static Dictionary<string, LeadUser> MsisdnCache = new Dictionary<string, LeadUser>();

    public async void Run(ClientDM client)
    {
      string email = client.email.ToLower().Replace("à", "a");
      if (string.IsNullOrEmpty(email))
        return;
      
      if (!client.GetCountry().HasValue)
        return;

      LanderDM lander = null;
      if (!string.IsNullOrEmpty(client.referrer))
      {
        string[] refSplit = client.referrer.Split('/');
        if (refSplit.Length >= 3)
        {
          string landingName = refSplit[3];
          if (Program.Landers.ContainsKey(landingName))
            lander = Program.Landers[landingName];
        }
      }

      int leadID = CurrentIndex;
      if (!EmailCache.ContainsKey(email) && !MsisdnCache.ContainsKey(client.msisdn))
      {
        var leadDM = new LeadDM(Program.Database)
        {
          email = email,
          msisdn = client.msisdn,
          first_name = client.firstname,
          last_name = client.lastname,
          countryid = client.GetCountry(),
          address = client.address,
          actions_count = 1,
          updated = client.created,
          created = client.created
        };
        leadDM.ID = CurrentIndex;
        Program.Bulker.Add(new BulkModel(leadDM, 1));

        var userDM = new UserDM(Program.Database)
        {
          leadid = CurrentIndex,
          countryid = client.GetCountry(),
          created = client.created
        };
        userDM.ID = CurrentIndex;
        Program.Bulker.Add(new BulkModel(userDM, 2));

        EmailCache.Add(email, new LeadUser() { ID = CurrentIndex });
        if(!string.IsNullOrEmpty(client.msisdn))
          MsisdnCache.Add(client.msisdn, new LeadUser() { ID = CurrentIndex });

        CurrentIndex++;
      }
      else if (EmailCache.ContainsKey(email))
      {
        leadID = EmailCache[email].ID;
        EmailCache[email].actions_count++;
      }
      else if (!string.IsNullOrEmpty(client.msisdn) && MsisdnCache.ContainsKey(client.msisdn))
      {
        leadID = MsisdnCache[client.msisdn].ID;
        MsisdnCache[client.msisdn].actions_count++;
      }

      var actionDM = new ActionDM(Program.Database)
      {
        leadid = leadID,
        userid = "", // TODO: get correct userID
        trackingid = client.clickid,
        affid = client.affid,
        pubid = client.pubid,
        landerid = (lander != null ? lander.ID : null),
        landertypeid = (lander != null ? (int?)lander.landertypeid : null),
        providerid = client.payment_provider,
        countryid = client.GetCountry(),
        input_redirect = true,
        input_email = (!string.IsNullOrEmpty(client.email)),
        input_contact = (!string.IsNullOrEmpty(client.firstname)),
        has_redirectedToProvider = true,
        has_subscription = client.has_subscription,
        has_refund = client.has_refund,
        has_chargeback = client.has_chargeback,
        has_stolen = client.is_stolen,
        times_charged = client.times_charged,
        times_upsell = client.times_upsell,
        updated = client.created,
        created = client.created
      };
      Program.Bulker.Add(new BulkModel(actionDM, 3));


    }

  }
}
