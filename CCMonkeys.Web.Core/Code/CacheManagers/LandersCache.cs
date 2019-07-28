using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Code.CacheManagers
{
  public class LanderTypeCacheModel
  {
    public int ID;
    public string Name;
  }

  public class LanderCacheModel
  {
    public int ID;
    public LanderTypeCacheModel Type;
    public string Name;
    public string Url;
  }

  public class LandersCache : CacheManagerBase
  {
    public static LandersCache Instance => (LandersCache)CacheManager.Get(CacheType.Lander);
    private static Dictionary<int, LanderTypeCacheModel> Types = new Dictionary<int, LanderTypeCacheModel>();
    private static Dictionary<int, LanderCacheModel> Landers = new Dictionary<int, LanderCacheModel>();
    

    protected override void Init()
    {
      foreach (var t in this.Database.Query<LanderTypeDM>().Where("[id]>0").LoadEnumerable())
        Types.Add(t.ID.Value, new LanderTypeCacheModel()
        {
          ID = t.ID.Value,
          Name = t.name
        });

      foreach (var l in this.Database.Query<LanderDM>().Where("[id]>0").LoadEnumerable())
        Landers.Add(l.ID.Value, new LanderCacheModel()
        {
          ID = l.ID.Value,
          Name = l.name,
          Type = Types[l.landertypeid],
          Url = l.url
        });
    }

    public LanderCacheModel Get(int id) => Landers.ContainsKey(id) ? Landers[id] : null;
    public LanderTypeCacheModel GetType(int id) => Types.ContainsKey(id) ? Types[id] : null;
    public List<LanderCacheModel> GetLandersModel() => (from l in Landers select l.Value).ToList();
    public List<LanderTypeCacheModel> GetLanderTypesModel() => (from l in Types select l.Value).ToList();
    public LanderCacheModel GetByUrl(string url) => (from l in Landers where l.Value.Url.Equals(url) select l.Value).FirstOrDefault();


  }
}
