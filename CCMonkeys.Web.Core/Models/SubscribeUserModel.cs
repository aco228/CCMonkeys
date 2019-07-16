using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models
{
  public class SubscribeUserModel
  {
    public int aid { get; set; } = -1;
    public int lid { get; set; } = -1;

    public string pp { get; set; } = "";
    public string firstname { get; set; } = "";
    public string lastname { get; set; } = "";
    public string msisdn { get; set; } = "";
    public string country { get; set; } = "";
    public string address { get; set; } = "";
    public string city { get; set; } = "";
    public string zip { get; set; } = "";
    public string cc_number { get; set; } = "";
    public string cc_expiry_month { get; set; } = "";
    public string cc_expiry_year { get; set; } = "";
    public string ccv { get; set; } = "";
    public string time { get; set; } = "";
    public string gacid { get; set; } = "";
    public string lxid { get; set; } = "";
    public string url { get; set; } = "";
    public int? affid { get; set; }
    public string pubid { get; set; } = "";

  }
}
