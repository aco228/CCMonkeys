using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.ApiSockets.Data
{
  public class ReceivingRegistrationModel
  {
    public string url { get; set; } = string.Empty;
    public int? providerID { get; set; } = null;
  }

  public class SendingRegistrationModel : SendingObj
  {
    public LeadDM lead { get; set; } = null;
    public SessionDataDM sessionData { get; set; } = null;

    public bool leadHasSubscription { get; set; } = false;
    public int? userVisitCount { get; set; } = null;

    public int? actionID { get; set; } = null;
    public int? sessionID { get; set; } = null;
    public int? userID { get; set; } = null;
  }

}
