using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class InitDashboardModel : SendingObj
  {
    public int Privileges;
    public string AdminStatus;
    public List<ActionLiveModel> Actions;
    public List<string> DashboardSessions;
    public List<CountryCacheModel> Countries;
    public List<LanderTypeCacheModel> LanderTypes;
    public List<LanderCacheModel> Landers;
    public List<PreLanderTypeCacheModel> PrelanderTypes;
    public List<PreLanderCacheModel> Prelanders;
    public List<ProviderCacheModel> Providers;
  }
}
