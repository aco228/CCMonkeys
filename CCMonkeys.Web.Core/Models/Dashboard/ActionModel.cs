using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using Direct;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class ActionModelReceive
  {
    public int Limit { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public string[] Clickids { get; set; }
    public string[] ClickidsExclude { get; set; }

    public int[] Prelanders { get; set; }
    public int[] PrelandersExclude { get; set; }

    public int[] PrelanderTypes { get; set; }
    public int[] PrelanderTypesExclude { get; set; }

    public int[] Landers { get; set; }
    public int[] LandersExclude { get; set; }

    public int[] LanderTypes { get; set; }
    public int[] LanderTypesExclude { get; set; }

    public int[] Affids { get; set; }
    public int[] AffidsExclude { get; set; }

    public int[] Pubids { get; set; }
    public int[] PubidsExclude { get; set; }

    public int[] Countries { get; set; }
    public int[] CountriesExclude { get; set; }

    public int[] Providers { get; set; }
    public int[] ProvidersExclude { get; set; }

    public bool? HadCharged { get; set; } = null;
    public bool? HasSubscription { get; set; } = null;
    public bool? HasChargeback { get; set; } = null;
    public bool? HasRefund { get; set; } = null;
  }

  public class ActionModelSend : SendingObj
  {
    public bool IsOnline { get; set; } = false;
    public ActionDM Data { get; set; }
    public string Lander { get; set; }
    public string Prelander { get; set; }
    public ProviderCacheModel Provider { get; set; }
    public CountryCacheModel Country { get; set; }
    public List<ActionModelPrelanderData> PrelanderData { get; set; }

    public string actionid { get; set; }
    public string trackingid { get; set; }
    public string affid { get; set; }
    public string pubid { get; set; }
    public bool input_redirect { get; set; }
    public bool input_email { get; set; }
    public bool input_contact { get; set; }
    public bool has_subscription { get; set; }
    public bool has_chargeback { get; set; }
    public bool has_refund { get; set; }
    public int times_charged { get; set; }
    public int times_upsell { get; set; }
    public bool has_redirectedToProvider { get; set; }
    public bool has_stolen { get; set; }
    public DateTime updated { get; set; }
    public DateTime created { get; set; }

  }

  public class ActionModelPrelanderData
  {
    public bool q { get; set; }
    public bool hv { get; set; }
    public string n { get; set; }
  }

  public static class ActionModelSendHelper
  {
    public static ActionModelSend Pack(this ActionDM action, bool? isOnline = null)
      => new ActionModelSend()
      {
        IsOnline = (isOnline != null ? isOnline.Value : ApiSocketServer.IsActionOnline(action)),
        Lander = action.landerid == null ? null : LandersCache.Instance.Get(action.landerid.Value).Name,
        Prelander = action.prelanderid == null ? null : PrelandersCache.Instance.Get(action.prelanderid.Value).Name,
        Provider = action.providerid == null ? null : ProvidersCache.Instance.Get(action.providerid.Value),
        Country = action.countryid == null ? null : CountryCache.Instance.Get(action.countryid.Value),
        PrelanderData = (!string.IsNullOrEmpty(action.prelander_data) && action.prelanderid.HasValue) ? PrelandersCache.Instance.ConstructTagsForAction(action.prelanderid.Value, action.prelander_data) : null,

        actionid = action.actionid,
        trackingid = action.trackingid,
        affid = action.affid,
        pubid = action.pubid,
        input_redirect = action.input_redirect,
        input_email = action.input_email,
        input_contact = action.input_contact,
        has_subscription = action.has_subscription,
        has_chargeback = action.has_chargeback,
        has_refund = action.has_refund,
        times_charged = action.times_charged,
        times_upsell = action.times_upsell,
        has_redirectedToProvider = action.has_redirectedToProvider,
        has_stolen = action.has_stolen,
        updated = action.updated,
        created = action.created
      };


  }

}
