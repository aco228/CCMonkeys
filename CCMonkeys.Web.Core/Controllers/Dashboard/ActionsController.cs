using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Code.Filters;
using CCMonkeys.Web.Core.Models.Dashboard;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{

  [AllowCrossSiteAttribute]
  [Route("api/actions")]
  public class ActionsController : MainController
  {
    public ActionsController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    [HttpGet]
    public async Task<List<ActionModelSend>> GetActions([FromQuery]ActionModelReceive input)
    {
      var queryManager = this.Database.Query<ActionDM>();

      #region $ lists

      if (input.Clickids != null && input.Clickids.Length > 0)
        queryManager.AddWhere("trackingid IN ({0})", input.Clickids, 0);
      if (input.ClickidsExclude != null && input.ClickidsExclude.Length > 0)
        queryManager.AddWhere("trackingid NOT IN ({0})", input.ClickidsExclude, 0);

      #region # prelanders

      if (input.Prelanders != null && input.Prelanders.Length > 0)
        queryManager.AddWhere("prelanderid IN ({0})", input.Prelanders, 0);
      if (input.PrelandersExclude != null && input.PrelandersExclude.Length > 0)
        queryManager.AddWhere("prelanderid NOT IN ({0})", input.PrelandersExclude, 0);

      if (input.PrelanderTypes != null && input.PrelanderTypes.Length > 0)
        queryManager.AddWhere("prelandertypeid IN ({0})", input.PrelanderTypes, 0);
      if (input.PrelanderTypesExclude != null && input.PrelanderTypesExclude.Length > 0)
        queryManager.AddWhere("prelandertypeid NOT IN ({0})", input.PrelanderTypesExclude, 0);

      #endregion
      #region # landers

      if (input.Landers != null && input.Landers.Length > 0)
        queryManager.AddWhere("landerid IN ({0})", input.Landers, 0);
      if (input.LandersExclude != null && input.LandersExclude.Length > 0)
        queryManager.AddWhere("landerid NOT IN ({0})", input.LandersExclude, 0);

      if (input.LanderTypes != null && input.LanderTypes.Length > 0)
        queryManager.AddWhere("landertypeid IN ({0})", input.LanderTypes, 0);
      if (input.LanderTypesExclude != null && input.LanderTypesExclude.Length > 0)
        queryManager.AddWhere("landertypeid NOT IN ({0})", input.LanderTypesExclude, 0);

      #endregion
      #region # affid and pubid #

      if (input.Affids != null && input.Affids.Length > 0)
        queryManager.AddWhere("affid IN ({0})", input.Affids, 0);
      if (input.AffidsExclude != null && input.AffidsExclude.Length > 0)
        queryManager.AddWhere("affid NOT IN ({0})", input.AffidsExclude, 0);

      if (input.Pubids != null && input.Pubids.Length > 0)
        queryManager.AddWhere("pubid IN ({0})", input.Pubids, 0);
      if (input.PubidsExclude != null && input.PubidsExclude.Length > 0)
        queryManager.AddWhere("pubid NOT IN ({0})", input.PubidsExclude, 0);

      #endregion

      if (input.Countries != null && input.Countries.Length > 0)
        queryManager.AddWhere("countryid IN ({0})", input.Countries, 0);
      if (input.CountriesExclude != null && input.CountriesExclude.Length > 0)
        queryManager.AddWhere("countryid NOT IN ({0})", input.CountriesExclude, 0);

      if (input.Providers != null && input.Providers.Length > 0)
        queryManager.AddWhere("providerid IN ({0})", input.Providers, 0);
      if (input.ProvidersExclude != null && input.ProvidersExclude.Length > 0)
        queryManager.AddWhere("providerid NOT IN ({0})", input.ProvidersExclude, 0);

      #endregion

      if (input.HadCharged.HasValue)
        queryManager.AddWhere(input.HadCharged.Value ? "times_charged>0" : "times_charged=0", 0);
      if (input.HasChargeback.HasValue)
        queryManager.AddWhere("has_chargeback={0}", input.HasChargeback.Value, 0);
      if (input.HasRefund.HasValue)
        queryManager.AddWhere("has_refund={0}", input.HasRefund.Value, 0);
      if (input.HasSubscription.HasValue)
        queryManager.AddWhere("has_subscription={0}", input.HasSubscription.Value, 0);
      if (input.From.HasValue)
        queryManager.AddWhere("created>={0}", input.From);
      if (input.To.HasValue)
        queryManager.AddWhere("created<={0}", input.To);

      
      queryManager.Additional("ORDER BY actionid DESC LIMIT " + input.Limit);

      List<ActionModelSend> result = new List<ActionModelSend>();
      foreach (var action in await queryManager.LoadAsync())
        result.Add(new ActionModelSend()
        {
          Data = action,
          Lander = action.landerid == null ? null : LandersCache.Instance.Get(action.landerid.Value),
          Prelander = action.prelanderid == null ? null : PrelandersCache.Instance.Get(action.prelanderid.Value),
          Provider = action.providerid == null ? null : ProvidersCache.Instance.Get(action.providerid.Value),
          Country = action.countryid == null ? null : CountryCache.Instance.Get(action.countryid.Value)
        });
      return result;
    }

  }
}
