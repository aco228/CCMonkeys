using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class ReceivingUserRedirected : ReceiveModel
  {
    public string url { get; set; } = string.Empty;
  }

  public class SendingUserRedirected : SendingObj
  {

  }

}
