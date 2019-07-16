using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CCMonkeys.Web.Postbacks.Controllers.Cloverdale
{
  public class CloverdaleModel : PostbackModelBase
  {
    public string firstname { get; set; } = "";
    public string lastname { get; set; } = "";
    public string address { get; set; } = "";
    public string email { get; set; } = "";
    public string zip { get; set; } = "";
    public string country { get; set; } = "";
    public string city { get; set; } = "";
    public string msisdn { get; set; } = "";
    public string clickid { get; set; } = "";
    public string pubid { get; set; } = "";
    public string _event { get; set; } = "";
    public string offer { get; set; } = "empty";
    public string affid { get; set; } = "";

    protected override void Init(HttpRequest Request)
    {
      this._event = Request.Query["event"];
      this.Type = this._event.Equals("settle") ? ActionModelEvent.Charge : ActionModelEvent.Create;
    }

    protected override string GetTrackingGuid()
      => this.clickid;

    protected override ActionModelEvent GetType()
      => this.Type;

  }
}
