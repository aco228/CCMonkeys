using CCMonkeys.Web.Core.Code.CacheManagers;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Code
{
  public class DomainManager
  {
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool HasError { get; private set; } = false;

    public string Url { get; set; } = string.Empty;
    public Dictionary<string, string> Queries { get; set; } = new Dictionary<string, string>();

    // CASE: Prelander
    public PrelanderDomainDM Domain { get; set; } = null;
    public PreLanderCacheModel Prelander { get; set; } = null;

    // CASE: Lander
    public LanderCacheModel Lander { get; set; } = null;
    public ProviderCacheModel Provider { get; set; } = null;


    public DomainManager() { }
    //public DomainManager(string url)
    //{
    //  //this.Url = url.Split('?')[0];
    //  //PrelandersCache cache = CacheManager.Get(CacheType.Prelander) as PrelandersCache;

    //  //// get domain
    //  //this.Domain = (from d in PrelandersCache.Domains where this.Url.StartsWith(d.Value.url) select d.Value).FirstOrDefault();
    //  //if(this.Domain == null)
    //  //{
    //  //  this.HasError = true;
    //  //  return;
    //  //}

    //  //// get prelander
    //  //string prelanderName = this.Url.Replace(this.Domain.url, string.Empty);
    //  //this.Prelander = (from d in PrelandersCache.Landers where d.Value.Url.Equals(prelanderName) select d.Value).FirstOrDefault();
    //  //if(this.Prelander == null)
    //  //{
    //  //  this.HasError = true;
    //  //  return;
    //  //}
    //  //string query = string.Empty;
    //  //if (url.Contains('?'))
    //  //  query = url.Split('?')[1];
    //  //var querySplit = query.Split('&');
    //  //if (querySplit.Length > 0)
    //  //  this.Queries = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);

    //  //this.HasError = false;
    //}

    // shared initialization
    private static DomainManager Initiate(string url)
    {
      DomainManager result = new DomainManager();
      result.Url = url.Split('?')[0];

      string query = string.Empty;
      if (url.Contains('?'))
        query = url.Split('?')[1];
      var querySplit = query.Split('&');
      if (querySplit.Length > 0)
        result.Queries = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);

      return result;
    }

    public static DomainManager InitiatePrelander(string url)
    {
      DomainManager result = Initiate(url);
      PrelandersCache cache = CacheManager.Get(CacheType.Prelander) as PrelandersCache;

      // get domain
      result.Domain = (from d in PrelandersCache.Domains where result.Url.StartsWith(d.Value.url) select d.Value).FirstOrDefault();
      if (result.Domain == null)
      {
        result.ErrorMessage = "Domain is null";
        result.HasError = true;
        return result;
      }

      // get prelander
      string prelanderName = result.Url.Replace(result.Domain.url, string.Empty);
      result.Prelander = (from d in PrelandersCache.Landers where d.Value.Url.Equals(prelanderName) select d.Value).FirstOrDefault();
      if (result.Prelander == null)
      {
        result.ErrorMessage = "Prelander could not be found";
        result.HasError = true;
        return result;
      }

      result.HasError = false;
      return result;
    }


    public static DomainManager InitiateLander(string url)
    {
      DomainManager result = Initiate(url);

      // check if there is provider id
      if (!result.Queries.ContainsKey("ptype"))
      {
        result.ErrorMessage = "ptype there is not";
        result.HasError = true;
        return result;
      }

      result.Lander = LandersCache.Instance.GetByUrl(result.Url);
      if (result.Lander == null)
      {
        result.ErrorMessage = "Lander could not be found";
        result.HasError = true;
        return result;
      }

      result.Provider = ProvidersCache.Instance.GetByKey(result.Queries["ptype"]);
      if (result.Provider == null)
      {
        result.ErrorMessage = "Provider is null";
        result.HasError = true;
        return result;
      }

      return result;
    }

  }
}
