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
    public string Url { get; set; } = string.Empty;
    public bool HasError { get; private set; } = false;
    public PrelanderDomainDM Domain { get; set; } = null;
    public PreLanderCacheModel Prelander { get; set; } = null;
    public Dictionary<string, string> Queries { get; set; } = new Dictionary<string, string>();

    public DomainManager(string url)
    {
      this.Url = url.Split('?')[0];
      PrelandersCache cache = CacheManager.Get(CacheType.Prelander) as PrelandersCache;

      // get domain
      this.Domain = (from d in PrelandersCache.Domains where this.Url.StartsWith(d.Value.url) select d.Value).FirstOrDefault();
      if(this.Domain == null)
      {
        this.HasError = true;
        return;
      }

      // get prelander
      string prelanderName = this.Url.Replace(this.Domain.url, string.Empty);
      this.Prelander = (from d in PrelandersCache.Landers where d.Value.Url.Equals(prelanderName) select d.Value).FirstOrDefault();
      if(this.Prelander == null)
      {
        this.HasError = true;
        return;
      }
      string query = string.Empty;
      if (url.Contains('?'))
        query = url.Split('?')[1];
      var querySplit = query.Split('&');
      if (querySplit.Length > 0)
        this.Queries = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);

      this.HasError = false;
    }

  }
}
