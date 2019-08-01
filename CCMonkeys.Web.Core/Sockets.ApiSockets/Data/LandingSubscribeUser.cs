using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class ReceivingSubscribeUser : ReceiveModel
  {
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string postcode { get; set; } = string.Empty;
    public string msisdn { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
  }

  public class SendingSubscribeUser : SendingObj
  {
    public bool internalError_leadDoesNotExists { get; set; } = false;
  }

}
