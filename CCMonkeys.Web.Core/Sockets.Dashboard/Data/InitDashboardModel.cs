using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class InitDashboardModel : SendingObj
  {
    public List<string> Actions;
    public List<string> DashboardSessions;
    public List<CountryCacheModel> Countries;
    public List<LanderTypeCacheModel> LanderTypes;
    public List<LanderCacheModel> Landers;
    public List<PreLanderTypeCacheModel> PrelanderTypes;
    public List<PreLanderCacheModel> Prelanders;
    public List<ProviderCacheModel> Providers;
  }
}
