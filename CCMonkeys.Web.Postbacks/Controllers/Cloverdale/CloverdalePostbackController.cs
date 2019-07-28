using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.CacheManagers;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Postbacks.Controllers.Cloverdale
{
  [Route("postback/cloverdale")]
  public class CloverdalePostbackController : PostbackControllerBase<CloverdaleModel>
  {
    public CloverdalePostbackController(int providerID, bool requireAction) : base(providerID:5, requireAction:false){ }

    protected override async Task<ActionDM> Call(CloverdaleModel model)
    {
      if (this.Action == null)
        this.Action = await this.ConstructAction(model);

      return this.Action;
    }

    // Landing pages are on CCCash side
    private async Task<ActionDM> ConstructAction(CloverdaleModel model)
    {
      var countryid = await CountryCache.Instance.Get(this.Database, model.country);

      var lead = await LeadDM.LoadByMsisdnOrEmailAsync(this.Database, model.msisdn, model.email);
      if (lead == null)
        lead = await new LeadDM(this.Database)
        {
          email = model.email,
          first_name = model.firstname,
          last_name = model.lastname,
          msisdn = model.msisdn,
          address = model.address,
          zip = model.zip,
          city = model.city,
          countryid = countryid,
          actions_count = 1
        }.InsertAsync<LeadDM>();

      var user = await new UserDM(this.Database)
      {
        countryid = countryid,
        leadid = lead.ID.Value
      }.InsertAsync<UserDM>();

      var action = await new ActionDM(this.Database)
      {
        trackingid = model.TrackingID,
        userid = user.GetStringID(),
        leadid = lead.ID.Value,
        affid = model.affid,
        pubid = model.pubid,
        countryid = countryid,
        input_redirect = true,
        input_email = true,
        input_contact = true,
        has_redirectedToProvider = true
      }.InsertAsync<ActionDM>();

      return action;
    }
  }
}
