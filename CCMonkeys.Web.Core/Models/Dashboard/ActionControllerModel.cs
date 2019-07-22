using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class ActionControllerModel
  {
    public int Limit { get; set; }
    public string[] Clickids { get; set; }
    public string[] Prelanders { get; set; }
    public string[] Landers { get; set; }
    public string[] Affids { get; set; }
    public string[] Pubids { get; set; }
    public string[] Countries { get; set; }
    public string[] Providers { get; set; }
    public bool? HadCharged { get; set; } = null;
    public bool? HasSubscription { get; set; } = null;
    public bool? HasChargeback { get; set; } = null;
    public bool? HasRefund { get; set; } = null;

    public string ConstructQuery()
    {
      string result = string.Empty;


      return result;
    }

  }
}
