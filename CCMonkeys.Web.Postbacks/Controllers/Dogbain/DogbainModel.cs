using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CCMonkeys.Web.Postbacks.Controllers.Dogbain
{
  public class DogbainModel : PostbackModelBase
  {
    public string txid { get; set; } = "";
    public string zone { get; set; } = "";
    public string ievent { get; set; } = "";
    public string email { get; set; } = "";
    public string username { get; set; } = "";
    public string password { get; set; } = "";

    protected override void Init(HttpRequest Request)
    {
      this.ievent = Request.Query["event"].ToString();
      if (ievent.Equals("initial"))
        ievent = "settle";

      this.Type = ActionModelEvent.Create;
      if (ievent.Equals("subscribed"))
        this.Type = ActionModelEvent.Subscribe;
      if (ievent.Equals("settle"))
        this.Type = ActionModelEvent.Charge;
    }

    protected override string GetTrackingGuid()
      => this.txid;

    protected override ActionModelEvent GetType()
      => this.Type;

  }
}
