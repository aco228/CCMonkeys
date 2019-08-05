using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core
{
  public enum CacheType { Country, Lander, Prelander, Providers }

  public static class CacheManager
  {
    private static object LockObj = new object();
    public static bool IsInitiated { get; set; } = false;
    private static CCSubmitDirect Database { get; set; }
    private static Dictionary<CacheType, CacheManagerBase> Managers { get; set; } = new Dictionary<CacheType, CacheManagerBase>();

    public static CacheManagerBase Get(CacheType type) => Managers.ContainsKey(type) ? Managers[type] : null;

    public static void Init()
    {
      lock(LockObj)
      {
        if (IsInitiated)
          return;

        try
        {
          Database = new CCSubmitDirect();
          Managers.Add(CacheType.Country, new CountryCache());
          Managers.Add(CacheType.Lander, new LandersCache());
          Managers.Add(CacheType.Prelander, new PrelandersCache());
          Managers.Add(CacheType.Providers, new ProvidersCache());

          foreach (var c in Managers)
            c.Value.Construct(Database);

          Database.TransactionalManager.RunAsync();
          IsInitiated = true;
        }
        catch (Exception e)
        {
          Loggings.Logger.Instance.StartLoggin("cachemanager")
            .OnException(e);
        }
      }
    }

  }
}
