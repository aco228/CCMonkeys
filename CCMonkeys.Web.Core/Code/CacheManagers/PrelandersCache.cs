using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMonkeys.Web.Core.Models.Dashboard;

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
    public List<PrelanderTagDM> Tags = null;
    public List<PrelanderTagAnswerDM> Answers = null;
    public string Name;
    public string Url;
  }

  public class PrelandersCache : CacheManagerBase
  {

    public static PrelandersCache Instance
    {
      get
      {
        if (!CacheManager.IsInitiated)
          CacheManager.Init();
        return (PrelandersCache)CacheManager.Get(CacheType.Prelander);
      }
    }

    private static Dictionary<int, PreLanderTypeCacheModel> Types = new Dictionary<int, PreLanderTypeCacheModel>();
    private static Dictionary<int, PreLanderCacheModel> Landers = new Dictionary<int, PreLanderCacheModel>();

    protected override async void Init()
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
          Tags = await this.Database.Query<PrelanderTagDM>().Where("prelanderid={0}", l.ID.Value).LoadAsync(),
          Answers = await this.Database.Query<PrelanderTagAnswerDM>().Where("prelanderid={0}", l.ID.Value).LoadAsync(),
          Url = l.url
        });
        
    }

    public PreLanderCacheModel Get(int id) => Landers.ContainsKey(id) ? Landers[id] : null;
    public PreLanderTypeCacheModel GetType(int id) => Types.ContainsKey(id) ? Types[id] : null;
    public List<PreLanderCacheModel> GetPrelandersModel() => (from l in Landers select l.Value).ToList();
    public List<PreLanderTypeCacheModel> GetPrelanderTypesModel() => (from l in Types select l.Value).ToList();
    public PreLanderCacheModel GetByUrl(string url) => (from l in Landers where l.Value.Url.Equals(url) select l.Value).FirstOrDefault();

    public PrelanderTagDM GetTag(int prelanderID, string tagName)
    {
      if (!Landers.ContainsKey(prelanderID))
        return null;

      foreach (var tag in Landers[prelanderID].Tags)
        if (tag.name.Equals(tagName))
          return tag;

      return null;
    }
    public PrelanderTagAnswerDM GetAnswer(int prelanderID, string tagName, string answerID)
    {
      if (!Landers.ContainsKey(prelanderID))
        return null;

      foreach (var tag in Landers[prelanderID].Answers)
        if (tag.tagName.Equals(tagName) && tag.name.Equals(answerID))
          return tag;

      return null;
    }

    public List<ActionModelPrelanderData> ConstructTagsForAction(int prelanderID, string cache)
    {
      if (string.IsNullOrEmpty(cache))
        return null;
      if (!Landers.ContainsKey(prelanderID))
        return null;

      List<ActionModelPrelanderData> result = new List<ActionModelPrelanderData>();
      string[] split = cache.Split('.');

      foreach(string tag in split)
      {
        if (string.IsNullOrEmpty(tag))
          continue;

        string[] values = tag.Split('=');
        if (values.Length != 2)
          continue;

        var tagObj = GetTag(prelanderID, values[0]);
        if (tagObj == null)
          continue;
        
        result.Add(new ActionModelPrelanderData()
        {
          q = tagObj.isQuestion,
          hv = !values[1].Equals("0"),
          n = tagObj.name
        });
      }
      return result;
    }

  }
}
