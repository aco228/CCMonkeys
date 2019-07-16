using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Postbacks.Controllers.Invoreak
{
  [Route("postback/invoreak")]
  public class InvoreakPostbackController : PostbackControllerBase<InvoreakModel>
  {
    public InvoreakPostbackController(int providerID, bool requireAction) : base(providerID:3, requireAction:false)
    { }

    protected override async Task<ActionDM> Call(InvoreakModel model)
    {

      if (model.result.Equals("1"))
      {
        this.Postback.Log("Invoreak returned result that is not 1");
        return null;
      };

      return this.Action;
    }
  }
}
