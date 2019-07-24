using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class AdminConnectedModel : SendingObj
  {
    public string Username { get; set; } = string.Empty;
  }

  public class AdminDisconnectedModel : SendingObj
  {
    public string Username { get; set; } = string.Empty;
  }
}
