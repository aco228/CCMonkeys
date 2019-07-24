using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
    public LanderCacheModel Lander { get; set; }
    public PreLanderCacheModel Prelander { get; set; }
    public ProviderCacheModel Provider { get; set; }
    public CountryCacheModel Country { get; set; }
  }

  public static class ActionModelSendHelper
  {
    public static ActionModelSend Pack(this ActionDM action, bool? isOnline = null)
      => new ActionModelSend()
      {
        IsOnline = (isOnline != null ? isOnline.Value : ApiSocketServer.IsActionOnline(action)),
        Data = action,
        Lander = action.landerid == null ? null : LandersCache.Instance.Get(action.landerid.Value),
        Prelander = action.prelanderid == null ? null : PrelandersCache.Instance.Get(action.prelanderid.Value),
        Provider = action.providerid == null ? null : ProvidersCache.Instance.Get(action.providerid.Value),
        Country = action.countryid == null ? null : CountryCache.Instance.Get(action.countryid.Value)
      };
  }

}
