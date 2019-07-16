using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models
{
  public class RedirectionModel
  {
    public int? aid { get; set; } = null;
    public int? lid { get; set; } = null;
    public int? cid { get; set; } = null;

    public int landerid { get; set; } = -1;
    public int? affid { get; set; } = null;
    public string pubid { get; set; } = string.Empty;
    public string clickid { get; set; } = string.Empty;

    public string country { get; set; } = string.Empty;

    public string url { get; set; } = string.Empty;
  }
}
