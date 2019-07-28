using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Code.CacheManagers
{

  public class PreLanderTypeCacheModel
  {
    public int ID;
    public string Name;
  }

  public class PreLanderCacheModel
  {
    public int ID;
    public PreLanderTypeCacheModel Type;
    public string Name;
    public string Url;
  }

  public class PrelandersCache : CacheManagerBase
  {

    public static PrelandersCache Instance => (PrelandersCache)CacheManager.Get(CacheType.Prelander);
    private static Dictionary<int, PreLanderTypeCacheModel> Types = new Dictionary<int, PreLanderTypeCacheModel>();
    private static Dictionary<int, PreLanderCacheModel> Landers = new Dictionary<int, PreLanderCacheModel>();


    protected override void Init()
    {
      foreach (var t in this.Database.Query<PrelanderTypeDM>().Where("[id]>0").LoadEnumerable())
        Types.Add(t.ID.Value, new PreLanderTypeCacheModel()
        {
          ID = t.ID.Value,
          Name = t.name
        });

      foreach (var l in this.Database.Query<PrelanderDM>().Where("[id]>0").LoadEnumerable())
        Landers.Add(l.ID.Value, new PreLanderCacheModel()
        {
          ID = l.ID.Value,
          Name = l.name,
          Type = Types[l.prelandertypeid],
          Url = l.url
        });
    }



    public PreLanderCacheModel Get(int id) => Landers.ContainsKey(id) ? Landers[id] : null;
    public PreLanderTypeCacheModel GetType(int id) => Types.ContainsKey(id) ? Types[id] : null;
    public List<PreLanderCacheModel> GetPrelandersModel() => (from l in Landers select l.Value).ToList();
    public List<PreLanderTypeCacheModel> GetPrelanderTypesModel() => (from l in Types select l.Value).ToList();
    public PreLanderCacheModel GetByUrl(string url) => (from l in Landers where l.Value.Url.Equals(url) select l.Value).FirstOrDefault();

  }
}
