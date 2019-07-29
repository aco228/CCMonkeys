using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Code.Filters;
using CCMonkeys.Web.Core.Models.Dashboard;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [Route("api/init")]
  public class InitializationController : DashboardController
  {
    public InitializationController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }


    [EnableCors("get")]
    [HttpGet("getAll")]
    public async Task<InitializationModelResponse> GetAll()
      => new InitializationModelResponse()
      {
        Countries = CountryCache.Instance.GetModel(),
        Landers = LandersCache.Instance.GetLandersModel(),
        LanderTypes = LandersCache.Instance.GetLanderTypesModel(),
        Prelanders = PrelandersCache.Instance.GetPrelandersModel(),
        PrelanderTypes = PrelandersCache.Instance.GetPrelanderTypesModel(),
        Providers = ProvidersCache.Instance.GetAll()
      };

    [EnableCors("get")]
    [HttpGet("getCountries")]
    public async Task<List<CountryCacheModel>> GetCountries()
      => CountryCache.Instance.GetModel();

    [EnableCors("get")]
    [HttpGet("getLanderTypes")]
    public async Task<List<LanderTypeCacheModel>> GetLanderTypes()
      => LandersCache.Instance.GetLanderTypesModel();

    [EnableCors("get")]
    [HttpGet("getLanders")]
    public async Task<List<LanderCacheModel>> GetLanders()
      => LandersCache.Instance.GetLandersModel();

    [EnableCors("get")]
    [HttpGet("getPrelanderTypes")]
    public async Task<List<PreLanderTypeCacheModel>> GetPrelanderTypes()
      => PrelandersCache.Instance.GetPrelanderTypesModel();

    [EnableCors("get")]
    [HttpGet("getPrelanders")]
    public async Task<List<PreLanderCacheModel>> GetPrelanders()
      => PrelandersCache.Instance.GetPrelandersModel();

    [EnableCors("get")]
    [HttpGet("getProviders")]
    public async Task<List<ProviderCacheModel>> GetProviders()
      => ProvidersCache.Instance.GetAll();




  }
}
