using Direct.ccmonkeys.Models;
using Direct.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Postbacks.Controllers.CCCash
{
  public class CCCashModel : PostbackModelBase
  {
    public string firstname { get; set; } = "";
    public string lastname { get; set; } = "";
    public string address { get; set; } = "";
    public string email { get; set; } = "";
    public string zip { get; set; } = "";
    public string country { get; set; } = "";
    public string city { get; set; } = "";
    public string clickid { get; set; } = "";
    public string subtracking { get; set; } = "";
    public string _event { get; set; } = "";

    public int aid { get; set; } = -1;
    public string affid { get; set; } = "";
    public string offer { get; set; } = "";
    public string pubid { get; set; } = "";
    public string msisdn { get; set; } = "";

    protected override void Init(HttpRequest Request)
    {
      this._event = Request.Query["event"].ToString();

      // load data from subtracking
      if (!string.IsNullOrEmpty(this.subtracking))
      {
        string[] subtracking_info = this.subtracking.Split('_');
        if (subtracking_info.Length == 0)
          return;

        if (subtracking_info.Length > 0)
          this.offer = subtracking_info[0];

        if (subtracking_info.Length > 1)
          this.affid = subtracking_info[1];

        if (subtracking_info.Length > 2)
          this.pubid = subtracking_info[2];

        if (subtracking_info.Length > 3)
          this.msisdn = subtracking_info[3];

        int aid_num = -1;
        if (subtracking_info.Length > 4 && int.TryParse(subtracking_info[4], out aid_num))
          this.aid = aid_num;
      }
    }


    protected override string GetTrackingGuid()
      => this.clickid;

    protected override ActionModelEvent GetType()
      => this._event.Equals("join") ? ActionModelEvent.Charge : ActionModelEvent.Create;

  }
}
