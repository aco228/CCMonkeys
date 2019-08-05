using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Code.CacheManagers
{

  public class ProviderCacheModel
  {
    public int ID;
    public string Name;
  }

  public class ProvidersCache : CacheManagerBase
  {
    public static ProvidersCache Instance
    {
      get
      {
        if (!CacheManager.IsInitiated)
          CacheManager.Init();
        return (ProvidersCache)CacheManager.Get(CacheType.Providers);
      }
    }
    private static Dictionary<int, ProviderCacheModel> Providers = new Dictionary<int, ProviderCacheModel>();

    protected override void Init()
    {
      foreach (var t in this.Database.Query<ProviderDM>().Where("[id]>0").LoadEnumerable())
        Providers.Add(t.ID.Value, new ProviderCacheModel()
        {
          ID = t.ID.Value,
          Name = t.name
        });
    }


    public ProviderCacheModel Get(int id) => Providers.ContainsKey(id) ? Providers[id] : null;
    public List<ProviderCacheModel> GetAll() => (from l in Providers select l.Value).ToList();

  }
}
