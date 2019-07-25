﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Postbacks.Controllers.Dogbain
{
  [Route("postback/dogbain")]
  public class DogbainPostbackController : PostbackControllerBase<DogbainModel>
  {
    public DogbainPostbackController() : base(providerID:2, requireAction:true)
    { }

    protected override async Task<ActionDM> Call(DogbainModel model)
    {

      if (!string.IsNullOrEmpty(model.username) && !string.IsNullOrEmpty(model.password))
        await new ActionAccountDM(this.Database)
        {
          actionid = this.Action.ID.Value,
          username = model.username,
          password = model.password
        }.InsertAsync<ActionAccountDM>();

      if (model.zone.ToLower().Contains("upsell"))
        this.Action.times_upsell++;

      return this.Action;
    }
  }
}
