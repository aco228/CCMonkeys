using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Logging;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class ReceivingRegistrationModel
  {
    public string url { get; set; } = string.Empty;
    public int? providerID { get; set; } = null;
  }

  public class SendingRegistrationModel : SendingObj
  {
    public LeadDM lead { get; set; } = null;
    public string country { get; set; } = string.Empty;

    public bool leadHasSubscription { get; set; } = false;
    public int? userVisitCount { get; set; } = null;
  }

  public class SendingRegistrationPost : SendingObj
  {
    public string sessionID = null;
    public string actionID = null;
    public string userID = null;
    public List<MSLoggerTrack> Loggers = null;
  }

}
