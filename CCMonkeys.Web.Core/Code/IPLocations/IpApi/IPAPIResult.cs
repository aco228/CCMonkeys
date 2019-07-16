using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.IPLocations.IpApi
{
 public class IPAPIResult
  {
    public string AS { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
    public string countryCode { get; set; } = string.Empty;
    public string isp { get; set; } = string.Empty;
    public string lat { get; set; } = string.Empty;
    public string lon { get; set; } = string.Empty;
    public string org { get; set; } = string.Empty;
    public string query { get; set; } = string.Empty;
    public string region { get; set; } = string.Empty;
    public string regionName { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public string timezone { get; set; } = string.Empty;
    public string zip { get; set; } = string.Empty;
  }
}
