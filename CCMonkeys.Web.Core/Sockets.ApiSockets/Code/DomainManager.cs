using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Code
{
  public class DomainManager
  {
    public string Domain { get; set; } = string.Empty;
    public Dictionary<string, string> Queries { get; set; } = new Dictionary<string, string>();

    public DomainManager(string url)
    {
      this.Domain = url.Split('?')[0];
      string query = string.Empty;
      if (url.Contains('?'))
        query = url.Split('?')[1];
      var querySplit = query.Split('&');
      if (querySplit.Length > 1)
        this.Queries = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);
    }

  }
}
