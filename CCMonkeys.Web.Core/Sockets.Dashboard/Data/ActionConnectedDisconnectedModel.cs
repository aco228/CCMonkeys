using CCMonkeys.Sockets;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class ActionUpdateModel : SendingObj
  {
    public ActionDM Data { get; set; }
  }

  public class ActionConnectedDisconnectedModel : SendingObj
  {
    public string ID { get; set; } = string.Empty;
  }
}
