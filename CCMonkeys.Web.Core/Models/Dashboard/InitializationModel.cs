using CCMonkeys.Web.Core.Code.CacheManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class InitializationModelResponse : ModelBaseResponse
  {
    public List<CountryCacheModel> Countries;
    public List<LanderTypeCacheModel> LanderTypes;
    public List<LanderCacheModel> Landers;
    public List<PreLanderTypeCacheModel> PrelanderTypes;
    public List<PreLanderCacheModel> Prelanders;
    public List<ProviderCacheModel> Providers;
  }
}
