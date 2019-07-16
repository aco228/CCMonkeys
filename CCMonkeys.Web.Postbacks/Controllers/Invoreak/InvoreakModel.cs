using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CCMonkeys.Web.Postbacks.Controllers.Invoreak
{
  public class InvoreakModel : PostbackModelBase
  {
    public string orderid { get; set; } = "";
    public string result { get; set; } = "";
    public string message { get; set; } = "";

    protected override void Init(HttpRequest Request)
    {
      this.Type = this.message.Equals("transaction successful") ? ActionModelEvent.Charge : ActionModelEvent.Subscribe;
    }

    protected override string GetTrackingGuid()
      => this.orderid;

    protected override ActionModelEvent GetType()
      => this.Type;

  }
}
